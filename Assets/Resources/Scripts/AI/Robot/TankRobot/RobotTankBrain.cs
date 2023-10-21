using System;
using NaughtyAttributes;
using UnityEngine;

namespace AI
{
    public class RobotTankBrain : RobotBrain
    {
        [BoxGroup("Idle")] [SerializeField, MinMaxSlider(0.0f, 15.0f)] private Vector2 _idleDelayRange;

        [SerializeField, BoxGroup("Patrolling")] private Transform[] _patrolPoints;
        [SerializeField, BoxGroup("Patrolling")] private float _patrolSpeed;

        [SerializeField, BoxGroup("Chasing")] private float _chaseSpeed;
        [SerializeField, BoxGroup("Chasing")] private float _minChaseDistance;

        [SerializeField, BoxGroup("Escape")] private float _escapeDistance;
        [SerializeField, BoxGroup("Escape")] private float _escapeSpeed;

        [SerializeField, BoxGroup("Combat")] private float _maxCombatDistance;
        [SerializeField, BoxGroup("Combat")] private TankAttackProperties _attackProperties;

        [SerializeField] private RobotState_Delay _delayState;
        [SerializeField] private RobotState_Patrol _patrolState;

        private TankRobotState_Chase _chaseState;
        private TankRobotState_Shoot _shootState;
        private TankRobotState_Escape _escapeState;
        
        protected override void Awake()
        {
            base.Awake();

            _stateMachine.SetState(_patrolState);
            _robotHealth.OnDeath.AddListener(() => Destroy(gameObject));
        }

        protected override void SetupStates()
        {
            _chaseState = new TankRobotState_Chase(_robotVision, _navMeshAgent, _chaseSpeed, _minChaseDistance, _maxCombatDistance);
            _escapeState = new TankRobotState_Escape
                (_escapeSpeed, _escapeDistance, 0, _navMeshAgent, _player);

            _shootState = new TankRobotState_Shoot
                (_player, _navMeshAgent, _robotHealth, _maxCombatDistance, _attackProperties);
        }

        protected override void SetupTransitions()
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
    }

    [Serializable]
    public struct TankAttackProperties
    {
        public Projectile Projectile => _projectile;
        public Transform ShootPoint => _shootPoint;
        public float AimSpeed => _aimSpeed;
        public float Damage => _damage;
        public float Speed => _attackSpeed;

        [SerializeField] private Projectile _projectile;
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private float _damage;
        [SerializeField] private float _attackSpeed;
        [SerializeField, Tooltip("deg/sec")] private float _aimSpeed;
    }
}
