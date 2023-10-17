using NaughtyAttributes;
using UnityEngine;

namespace AI
{
    public class HammerRobotBrain : RobotBrain
    {
        [SerializeField, BoxGroup("Death")] private float _destroyDelay;

        [BoxGroup("Idle")]
        [SerializeField, MinMaxSlider(0.0f, 15.0f)] private Vector2 _idleDelayRange;

        [SerializeField, BoxGroup("Patrolling")] private Transform[] _patrolPoints;
        [SerializeField, BoxGroup("Patrolling")] private float _patrolSpeed;

        [SerializeField, BoxGroup("Chasing")] private float _chaseSpeed;
        [SerializeField, BoxGroup("Chasing")] private float _chaseMinDistance;
        [SerializeField, BoxGroup("Chasing")] private float _chaseMaxDistance;

        [SerializeField, BoxGroup("Combat")] private RobotState_Combat _combatState;
        private RobotState_Delay _delayState;
        private RobotState_Patrol _patrolState;
        private RobotState_Chase _chaseState;
        private RobotState_Stan _stanState;
        private RobotState_Death _deathState;

        public bool IsInStan => _stateMachine.IsInState(_stanState);

        protected override void Awake()
        {
            base.Awake();

            _stateMachine.SetState(_patrolState);
        }

        protected override void SetupStates()
        {
            _delayState = new RobotState_Delay(_idleDelayRange);
            _patrolState = new RobotState_Patrol(_patrolSpeed, _navMeshAgent, _patrolPoints);
            _chaseState = new RobotState_Chase(_robotVision, _navMeshAgent, _chaseSpeed, _chaseMinDistance, _chaseMaxDistance);
            _stanState = new RobotState_Stan(_navMeshAgent, _combatState.StanDuration);
            _deathState = new RobotState_Death(_navMeshAgent, _destroyDelay);
        }

        protected override void SetupTransitions()
        {
            _stateMachine.AddTransition(_patrolState, _delayState, _patrolState.IsDone);
            _stateMachine.AddTransition(_delayState, _patrolState, _delayState.IsDone);

            _stateMachine.AddTransition(_delayState, _chaseState, _robotVision.IsVisible);
            _stateMachine.AddTransition(_patrolState, _chaseState, _robotVision.IsVisible);

            _stateMachine.AddTransition(_chaseState, _combatState, _chaseState.IsDone);
            _stateMachine.AddTransition(_chaseState, _delayState, _chaseState.IsLostPlayer);

            _stateMachine.AddTransition(_combatState, _delayState, _combatState.IsDone);
            _stateMachine.AddTransition(_combatState, _chaseState, _combatState.IsLostPlayer);

            _stateMachine.AddTransition(_stanState, _chaseState, _stanState.IsDone);

            _robotHealth.OnDeath.AddListener(OnDeath);
            _combatState.OnStan.AddListener(OnStan);
        }

        private void OnStan()
        {
            if (IsInStan)
                return;

            _stateMachine.SetState(_stanState);
        }

        private void OnDeath()
        {
            _stateMachine.SetState(_deathState);
        }
    }
}
