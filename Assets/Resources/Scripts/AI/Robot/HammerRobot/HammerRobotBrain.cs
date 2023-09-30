using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace AI
{
    public class HammerRobotBrain : RobotBrain
    {
        [HideInInspector] public UnityEvent<int> OnAttack;

        [Header("Death")]
        [Required, SerializeField] private RobotAnimator _animator;
        [SerializeField] private float _destroyDelay;

        [BoxGroup("Idle")]
        [SerializeField, MinMaxSlider(0.0f, 15.0f)] private Vector2 _idleDelayRange;

        [SerializeField, BoxGroup("Patrolling")] private Transform[] _patrolPoints;
        [SerializeField, BoxGroup("Patrolling")] private float _patrolSpeed;

        [SerializeField, BoxGroup("Chasing")] private float _chaseSpeed;
        [SerializeField, BoxGroup("Chasing")] private float _chaseMinDistance;
        [SerializeField, BoxGroup("Chasing")] private float _chaseMaxDistance;

        [SerializeField, BoxGroup("Combat")] private float _maxCombatDistance;
        [SerializeField, BoxGroup("Combat")] private float _stanDuration;
        [SerializeField, Range(0, 1), BoxGroup("Combat")] private float _stanChance;
        [SerializeField, BoxGroup("Combat")] private RobotState_Combat.AttacksProperties _attacksProperties;

        private RobotState_Delay _delayState;
        private RobotState_Patrol _patrolState;
        private RobotState_Chase _chaseState;
        private RobotState_Combat _combatState;
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
            _stanState = new RobotState_Stan(_stanDuration);
            _deathState = new RobotState_Death(gameObject, _destroyDelay);

            _combatState = new RobotState_Combat(_player, this, _maxCombatDistance, _attacksProperties);

            _combatState.OnPerformAttack += (index) => OnAttack?.Invoke(index);
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
            _robotHealth.OnTakeDamage.AddListener(OnTakeDamage);
        }

        private void OnTakeDamage()
        {
            if (_stateMachine.IsInState(_deathState))
                return;

            if (Random.Range(0f, 1f) > _stanChance)
                return;

            _stateMachine.SetState(_stanState);
        }

        private void OnDeath()
        {
            _stateMachine.SetState(_deathState);
        }
    }
}
