using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

namespace AI
{
    // ----------------------------------------
    // YOU NEED TO ADD *NAVMESH SURFACE COMPONENT* ON THE ROOM WHERE THIS ROBOT WILL BE LOCATED
    // ----------------------------------------
    public class RobotTankBrain : MonoBehaviour
    {
        [Required, SerializeField] private NavMeshAgent _navMeshAgent;
        [Required, SerializeField] private Player _player;

        [SerializeField] private RobotVision _robotVision;

        [BoxGroup("Idle")] [SerializeField, MinMaxSlider(0.0f, 15.0f)] private Vector2 _idleDelayRange;

        [SerializeField, BoxGroup("Patrolling")] private Transform[] _patrolPoints;
        [SerializeField, BoxGroup("Patrolling")] private float _patrolSpeed;

        [SerializeField, BoxGroup("Chasing")] private float _chaseSpeed;
        [SerializeField, BoxGroup("Chasing")] private float _minChaseDistance;

        [SerializeField, BoxGroup("Escape")] private float _escapeDisatnce;
        [SerializeField, BoxGroup("Escape")] private float _escapeSpeed;

        [SerializeField, BoxGroup("Combat")] private float _maxCombatDistance;

        [SerializeField, SOInheritedFrom(typeof(ITankRobotAttack)), BoxGroup("Combat")]
        private ScriptableObject _robotAttack; 
        // SOInheritedFrom attribute ensures that objects will inherit from IRobotAttack

        private StateMachine _stateMachine;
        private RobotState_Delay _delayState;
        private RobotState_Patrol _patrolState;
        private TankRobotState_Chase _chaseState;
        private TankRobotState_Shoot _shootState;
        private TankRobotState_Escape _escapeState;

        private const float ZeroDistance = 0f;
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
            _patrolState = new RobotState_Patrol(_patrolSpeed,_navMeshAgent, _patrolPoints);
            _chaseState = new TankRobotState_Chase(_robotVision, _navMeshAgent, _chaseSpeed, _minChaseDistance, _maxCombatDistance);
            _escapeState = new TankRobotState_Escape
                (_escapeSpeed, _escapeDisatnce, ZeroDistance, _navMeshAgent, _player);

            //List<IRobotAttack> attacks = _robotAttacks.ConvertAll(x => x as IRobotAttack);
            
            //_combatState = new TankRobotState_Combat(_player, _navMeshAgent, _maxCombatDistance, attacks);
        }

        private void SetupTransitions()
        {
            _stateMachine.AddTransition(_patrolState, _delayState, () => _patrolState.IsDone());
            _stateMachine.AddTransition(_delayState, _patrolState, () => _delayState.IsDone());

            _stateMachine.AddTransition(_delayState, _chaseState, () => _robotVision.IsVisible());
            _stateMachine.AddTransition(_patrolState, _chaseState, () => _robotVision.IsVisible());

            _stateMachine.AddTransition(_chaseState, _shootState, () => _chaseState.IsDone());
            _stateMachine.AddTransition(_chaseState, _delayState, () => _chaseState.IsLostPlayer());

            _stateMachine.AddTransition(_shootState, _delayState, () => _shootState.IsDone());
            _stateMachine.AddTransition(_shootState, _chaseState, () => _shootState.IsLostPlayer());
            _stateMachine.AddTransition(_shootState, _escapeState, () => _escapeState.IsPlayerClose());

            _stateMachine.AddTransition(_escapeState, _patrolState, () => _escapeState.IsPlayerFar());
        }

        private void OnDrawGizmosSelected()
        {
            _robotVision.OnGizmos();
        }
    }
}
