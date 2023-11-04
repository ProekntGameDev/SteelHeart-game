using System;
using NaughtyAttributes;
using UnityEngine;

namespace AI
{
    public class RobotTankBrain : RobotBrain
    {
        [SerializeField] private RobotState_Delay _delayState;
        [SerializeField] private RobotState_Patrol _patrolState;
        [SerializeField] private TankRobotState_Chase _chaseState;
        [SerializeField] private TankRobotState_Shoot _shootState;
        [SerializeField] private TankRobotState_Escape _escapeState;
        
        protected override void Awake()
        {
            base.Awake();

            _stateMachine.SetState(_patrolState);
            _robotHealth.OnDeath.AddListener(() => Destroy(gameObject));
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
