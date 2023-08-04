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

        [SerializeField, SOInheritedFrom(typeof(IRobotAttack)), BoxGroup("Combat")]
        private List<ScriptableObject> _robotAttacks = new List<ScriptableObject>(); 
        // SOInheritedFrom attribute ensures that objects will inherit from IRobotAttack
        
        private StateMachine _stateMachine;
        private TankRobotState_Idle _idleState;
        private TankRobotState_Patrol _patrolState;
        private TankRobotState_Chase _chaseState;
        private TankRobotState_Combat _combatState;
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
            _idleState = new TankRobotState_Idle(_idleDelayRange, _navMeshAgent, ZeroDistance);
            _patrolState = new TankRobotState_Patrol(_patrolSpeed, ZeroDistance,_navMeshAgent, _patrolPoints);
            _chaseState = new TankRobotState_Chase(_robotVision, _navMeshAgent, _chaseSpeed, _minChaseDistance, _maxCombatDistance);
            _escapeState = new TankRobotState_Escape
                (_escapeSpeed, _escapeDisatnce, ZeroDistance, _navMeshAgent, _player);
            
            List<IRobotAttack> attacks = _robotAttacks.ConvertAll(x => x as IRobotAttack);

            _combatState = new TankRobotState_Combat(_player, _navMeshAgent, _maxCombatDistance, attacks);
        }

        private void SetupTransitions()
        {
            _stateMachine.AddTransition(_patrolState, _idleState, () => _patrolState.IsDone());
            _stateMachine.AddTransition(_idleState, _patrolState, () => _idleState.IsDone());

            _stateMachine.AddTransition(_idleState, _chaseState, () => _robotVision.IsVisible());
            _stateMachine.AddTransition(_patrolState, _chaseState, () => _robotVision.IsVisible());

            _stateMachine.AddTransition(_chaseState, _combatState, () => _chaseState.IsDone());
            _stateMachine.AddTransition(_chaseState, _idleState, () => _chaseState.IsLostPlayer());
            
            _stateMachine.AddTransition(_combatState, _idleState, () => _combatState.IsDone());
            _stateMachine.AddTransition(_combatState, _chaseState, () => _combatState.IsLostPlayer());
            _stateMachine.AddTransition(_combatState, _escapeState, () => _escapeState.IsPlayerClose());
            
            _stateMachine.AddTransition(_escapeState, _patrolState, () => _escapeState.IsPlayerFar());
        }

        private void OnDrawGizmosSelected()
        {
            _robotVision.OnGizmos();
        }
    }
}

