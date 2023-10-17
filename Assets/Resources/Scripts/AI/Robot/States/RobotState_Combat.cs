using UnityEngine;
using UnityEngine.AI;
using System;
using NaughtyAttributes;
using Zenject;
using UnityEngine.Events;

using Random = UnityEngine.Random;

namespace AI
{
    public class RobotState_Combat : MonoBehaviour, IState
    {
        [HideInInspector] public UnityEvent OnStan;

        public float StanDuration => _stanDuration;

        [SerializeField,Required] private NavMeshAgent _navMeshAgent;
        [SerializeField, Required] private Health _robotHealth;
        [SerializeField] private float _maxDistance;
        [SerializeField] private float _stanDuration;
        [SerializeField, Range(0, 1)] private float _stanChance;

        [SerializeField, Required] private RobotAttack_Area _robotAreaAttack;
        [SerializeField, Required] private RobotAttack_Hammer _robotTargetAttack;

        [Inject] private Player _player;

        private StateMachine _stateMachine;

        public void OnEnter()
        { }

        public void OnExit()
        {
            if (_stateMachine.HasState == false)
                return;

            _stateMachine.SetState(null);
        }

        public void Tick()
        {
            _stateMachine.Tick();

            if (_stateMachine.HasState)
                return;

            float distanceToPlayer = Vector3.Distance(_player.transform.position, _navMeshAgent.transform.position);
            float playerSpeed = _player.Movement.CharacterController.CurrentVelocity.magnitude;

            if (distanceToPlayer <= _robotTargetAttack.AttackProperties.MaxDistance)
                _stateMachine.SetState(_robotTargetAttack);
            else if(distanceToPlayer <= _robotAreaAttack.AttackProperties.JumpDistance)
                _stateMachine.SetState(_robotAreaAttack);
            else
                _navMeshAgent.destination = _player.transform.position;

        }

        public bool IsDone() => _player.Health.Current == 0;

        public bool IsLostPlayer()
        {
            return Vector3.Distance(_player.transform.position, _navMeshAgent.transform.position) > _maxDistance;
        }

        private void Awake()
        {
            InitStateMachine();

            SetupTransitions();

            _robotHealth.OnTakeDamage.AddListener(OnTakeDamage);
        }

        private void InitStateMachine()
        {
            _stateMachine = new StateMachine();
        }

        private void SetupTransitions()
        {
            _stateMachine.AddTransition(_robotAreaAttack, null, _robotAreaAttack.IsDone);
            _stateMachine.AddTransition(_robotTargetAttack, null, _robotTargetAttack.IsDone);
        }

        private void OnTakeDamage(Damage damage)
        {
            if (_robotHealth.Current == 0)
                return;

            if (Random.Range(0f, 1f) > _stanChance)
                return;

            OnStan?.Invoke();
        }
    }
}
