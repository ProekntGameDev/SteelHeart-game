using NaughtyAttributes;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using System;

namespace AI
{
    public class RobotBrain : MonoBehaviour
    {
        [Required, SerializeField] private NavMeshAgent _navMeshAgent;
        [Required, SerializeField] private Player _player;

        [SerializeField] private RobotVision _robotVision;

        [BoxGroup("Idle")]
        [SerializeField, MinMaxSlider(0.0f, 15.0f)] private Vector2 _idleDelayRange;

        [SerializeField, BoxGroup("Patrolling")] private Transform[] _patrolPoints;
        [SerializeField, BoxGroup("Patrolling")] private float _patrolSpeed;

        [SerializeField, BoxGroup("Chasing")] private float _chaseSpeed;

        [SerializeField, BoxGroup("Combat")] private float _maxCombatDistance;
        [SerializeField, BoxGroup("Combat")] private RobotAttackProperties _firstAttackProperties;
        [SerializeField, BoxGroup("Combat")] private RobotAttackProperties _secondAttackProperties;

        private StateMachine _stateMachine;
        private RobotState_Delay _delayState;
        private RobotState_Patrol _patrolState;
        private RobotState_Chase _chaseState;
        private RobotState_Combat _combatState;

        private void Awake()
        {
            _stateMachine = new StateMachine();

            SetupStates();

            SetupTransitions();

            _stateMachine.SetState(_patrolState);
        }

        private void Update()
        {
            _stateMachine.Tick();
        }

        private void SetupStates()
        {
            _delayState = new RobotState_Delay(_idleDelayRange);
            _patrolState = new RobotState_Patrol(_patrolSpeed, _navMeshAgent, _patrolPoints);
            _chaseState = new RobotState_Chase(_robotVision, _navMeshAgent, _chaseSpeed, 2, 8);

            Dictionary<Type, RobotAttackProperties> attackProperties = new Dictionary<Type, RobotAttackProperties> 
            {
                { typeof(RobotAttack_Area), _firstAttackProperties },
                { typeof(RobotAttack_Hammer), _secondAttackProperties },
            };

            _combatState = new RobotState_Combat(_player, _navMeshAgent, _maxCombatDistance, attackProperties);
        }

        private void SetupTransitions()
        {
            _stateMachine.AddTransition(_patrolState, _delayState, () => _patrolState.IsDone());
            _stateMachine.AddTransition(_delayState, _patrolState, () => _delayState.IsDone());

            _stateMachine.AddTransition(_delayState, _chaseState, () => _robotVision.IsVisible());
            _stateMachine.AddTransition(_patrolState, _chaseState, () => _robotVision.IsVisible());

            _stateMachine.AddTransition(_chaseState, _combatState, () => _chaseState.IsDone());
            _stateMachine.AddTransition(_chaseState, _delayState, () => _chaseState.IsLostPlayer());

            _stateMachine.AddTransition(_combatState, _delayState, () => _combatState.IsDone());
            _stateMachine.AddTransition(_combatState, _chaseState, () => _combatState.IsLostPlayer());


            _stateMachine.AddAnyTransition(_combatState, () => _chaseState.IsLostPlayer());
        }

        private void OnDrawGizmosSelected()
        {
            _robotVision.OnGizmos();
        }
    }
}
