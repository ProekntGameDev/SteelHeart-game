using UnityEngine;

namespace AI
{
    public class HammerRobotBrain : RobotBrain
    {
        [Header("States")]
        [SerializeField] private RobotState_Finishing _finishingState;
        [SerializeField] private RobotState_Combat _combatState;
        [SerializeField] private RobotState_Patrol _patrolState;
        [SerializeField] private RobotState_Chase _chaseState;
        [SerializeField] private RobotState_Death _deathState;
        [SerializeField] private RobotState_Delay _delayState;
        [SerializeField] private RobotState_Stan _stanState;

        protected override void Awake()
        {
            base.Awake();

            _stateMachine.SetState(_patrolState);
        }

        protected override void SetupStates()
        { }

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

            _stateMachine.AddTransition(_finishingState, _deathState, _finishingState.IsDone);

            _robotHealth.OnDeath.AddListener(OnDeath);
            _combatState.OnStan.AddListener(OnStan);
        }

        private void OnStan()
        {
            if (_stateMachine.IsInState(_stanState) || _stateMachine.IsInState(_finishingState))
                return;

            _stanState.SetDuration(_combatState.StanDuration);
            _stateMachine.SetState(_stanState);
        }

        private void OnDeath()
        {
            _stateMachine.SetState(_finishingState);
        }
    }
}
