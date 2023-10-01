using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace AI
{
    public class DroneBehavior : RobotBrain
    {
        [HideInInspector] public UnityEvent<int> OnAttack;

        [SerializeField] private float _destroyDelay;

        [SerializeField, BoxGroup("Idle"), MinMaxSlider(0.0f, 15.0f)]
        private Vector2 _idleDelayRange;

        [SerializeField, BoxGroup("Patrolling")] private Transform[] _patrolPoints;
        [SerializeField, BoxGroup("Patrolling")] private float _patrolSpeed;

        [SerializeField, BoxGroup("Chasing")] private float _chaseSpeed;
        [SerializeField, BoxGroup("Chasing")] private float _chaseMinDistance;
        [SerializeField, BoxGroup("Chasing")] private float _chaseMaxDistance;

        [SerializeField, BoxGroup("Combat")] private float _maxCombatDistance;
        [SerializeField, BoxGroup("Combat")] private DroneAttackProperties _attackProperties;

        [SerializeField, BoxGroup("Freeze")] private float _freezeMinHeight;
        [SerializeField, BoxGroup("Freeze")] private float _freezeDuration;

        private RobotState_Delay _delayState;
        private RobotState_Patrol _patrolState;
        private RobotState_Chase _chaseState;
        
        private RobotState_Death _deathState;
        
        private DroneState_Attack _attackState;
        private DroneState_Freeze _freezeState;

        protected override void Awake()
        {
            base.Awake();

            _stateMachine.SetState(_patrolState);
        }

        protected override void SetupStates()
        {
            _delayState = new RobotState_Delay(_idleDelayRange);
            _patrolState = new RobotState_Patrol(_patrolSpeed, _navMeshAgent, _patrolPoints);
            _chaseState =
                new RobotState_Chase(_robotVision, _navMeshAgent, _chaseSpeed, _chaseMinDistance, _chaseMaxDistance);
            _deathState = new RobotState_Death(_navMeshAgent, _destroyDelay);
            _attackState = 
                new DroneState_Attack(_player, _navMeshAgent, _chaseMinDistance, _maxCombatDistance, _attackProperties);
            _freezeState = 
                new DroneState_Freeze(_navMeshAgent, _attackState, _freezeMinHeight, _freezeDuration);
        }

        protected override void SetupTransitions()
        {
            _stateMachine.AddTransition(_patrolState, _delayState, _patrolState.IsDone);
            _stateMachine.AddTransition(_delayState, _patrolState, _delayState.IsDone);

            _stateMachine.AddTransition(_delayState, _chaseState, _robotVision.IsVisible);
            _stateMachine.AddTransition(_patrolState, _chaseState, _robotVision.IsVisible);

            _stateMachine.AddTransition(_chaseState, _attackState, _chaseState.IsDone);
            _stateMachine.AddTransition(_chaseState, _delayState, _chaseState.IsLostPlayer);

            _stateMachine.AddTransition(_attackState, _delayState, _attackState.IsDone);
            _stateMachine.AddTransition(_attackState, _chaseState, _attackState.IsLostPlayer);
            _stateMachine.AddTransition(_attackState, _freezeState, _attackState.IsTimeOut);
            
            _stateMachine.AddTransition(_freezeState, _delayState, _freezeState.IsDone);
            
            _robotHealth.OnDeath.AddListener(OnDeath);
        }

        private void OnDeath()
        {
            _stateMachine.SetState(_deathState);
        }
    }

    [Serializable]
    public struct DroneAttackProperties
    {
        public Projectile Projectile => _projectile;
        public Transform ShootPoint => _shootPoint;
        public float AimSpeed => _aimSpeed;
        public float Damage => _damage;
        public float Speed => _attackSpeed;
        public int AttacksBeforeFreeze => _attacksBeforeFreeze;
        
        [SerializeField] private Projectile _projectile;
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private float _damage;
        [SerializeField] private float _attackSpeed;
        [SerializeField] private int _attacksBeforeFreeze;
        [SerializeField, Tooltip("deg/sec")] private float _aimSpeed;
    }
}
    

