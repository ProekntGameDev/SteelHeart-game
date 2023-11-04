using System;
using UnityEngine;
using UnityEngine.Events;

namespace AI
{
    public class DroneBehavior : RobotBrain
    {
        [HideInInspector] public UnityEvent<int> OnAttack;

        [SerializeField] private RobotState_Delay _delayState;
        [SerializeField] private RobotState_Patrol _patrolState;
        [SerializeField] private RobotState_Chase _chaseState;
        [SerializeField] private RobotState_Death _deathState;
        [SerializeField] private DroneState_Attack _attackState;
        [SerializeField] private DroneState_Freeze _freezeState;

        protected override void Awake()
        {
            base.Awake();

            _stateMachine.SetState(_patrolState);
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
    

