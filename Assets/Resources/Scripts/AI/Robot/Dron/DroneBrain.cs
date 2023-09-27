using System;
using AI;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Zenject;

namespace AI
{
    public class DroneBehavior : MonoBehaviour
    {
        [HideInInspector] public UnityEvent<int> OnAttack;

        [Required, SerializeField] private NavMeshAgent _navMeshAgent;
        [Required, SerializeField] private Health _robotHealth;

        [Header("Death")] [Required, SerializeField]
        private RobotAnimator _animator;

        [SerializeField] private float _destroyDelay;

        [SerializeField] private RobotVision _robotVision;

        [BoxGroup("Idle")] [SerializeField, MinMaxSlider(0.0f, 15.0f)]
        private Vector2 _idleDelayRange;

        [SerializeField, BoxGroup("Patrolling")]
        private Transform[] _patrolPoints;

        [SerializeField, BoxGroup("Patrolling")]
        private float _patrolSpeed;

        [SerializeField, BoxGroup("Chasing")] private float _chaseSpeed;
        [SerializeField, BoxGroup("Chasing")] private float _chaseMinDistance;
        [SerializeField, BoxGroup("Chasing")] private float _chaseMaxDistance;

        [SerializeField, BoxGroup("Combat")] private float _maxCombatDistance;
        [SerializeField, BoxGroup("Combat")] private DroneAttackProperties _attackProperties;
        //[SerializeField, BoxGroup("Combat")] private float _stanDuration;
        //[SerializeField, BoxGroup("Combat")] private RobotState_Combat.AttacksProperties _attacksProperties;

        [Inject] private Player _player;

        private StateMachine _stateMachine;

        private RobotState_Delay _delayState;
        private RobotState_Patrol _patrolState;
        private RobotState_Chase _chaseState;
        private DroneState_Attack _attackState;
        private RobotState_Death _deathState;

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
            _chaseState =
                new RobotState_Chase(_robotVision, _navMeshAgent, _chaseSpeed, _chaseMinDistance, _chaseMaxDistance);
            _deathState = new RobotState_Death(gameObject, _destroyDelay);
            _attackState = new DroneState_Attack(_player, _navMeshAgent, _maxCombatDistance, _attackProperties);
            //_combatState = new RobotState_Combat(_player, this, _maxCombatDistance, _attacksProperties);
            //_shootState.OnPerformAttack += (index) => OnAttack?.Invoke(index);
        }

        private void SetupTransitions()
        {
            _stateMachine.AddTransition(_patrolState, _delayState, _patrolState.IsDone);
            _stateMachine.AddTransition(_delayState, _patrolState, _delayState.IsDone);

            _stateMachine.AddTransition(_delayState, _chaseState, _robotVision.IsVisible);
            _stateMachine.AddTransition(_patrolState, _chaseState, _robotVision.IsVisible);

            _stateMachine.AddTransition(_chaseState, _attackState, _chaseState.IsDone);
            _stateMachine.AddTransition(_chaseState, _delayState, _chaseState.IsLostPlayer);

            _stateMachine.AddTransition(_attackState, _delayState, _attackState.IsDone);
            _stateMachine.AddTransition(_attackState, _chaseState, _attackState.IsLostPlayer);

            _robotHealth.OnDeath.AddListener(OnDeath);
            //_robotHealth.OnTakeDamage.AddListener(OnTakeDamage);
        }

        private void OnDeath()
        {
            _stateMachine.SetState(_deathState);
        }

        private void OnDrawGizmosSelected()
        {
            _robotVision.OnGizmos();
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

        [SerializeField] private Projectile _projectile;
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private float _damage;
        [SerializeField] private float _attackSpeed;
        [SerializeField, Tooltip("deg/sec")] private float _aimSpeed;
    }
}
    

