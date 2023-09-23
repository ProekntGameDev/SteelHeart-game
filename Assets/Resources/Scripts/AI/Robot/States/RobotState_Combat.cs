using UnityEngine;
using UnityEngine.AI;
using System;

namespace AI
{
    public class RobotState_Combat : IState
    {
        public event Action<int> OnPerformAttack;

        private readonly NavMeshAgent _navMeshAgent;
        private readonly AttacksProperties _attackProperties;
        private readonly float _maxDistance;
        private readonly Player _player;
        private readonly Health _robotHealth;

        private StateMachine _stateMachine;
        private RobotAttack_Area _robotAreaAttack;
        private RobotAttack_Hammer _robotTargetAttack;

        public RobotState_Combat(Player player, HammerRobotBrain robotTankBrain, float maxDistance, AttacksProperties attackProperties)
        {
            _navMeshAgent = robotTankBrain.GetComponent<NavMeshAgent>();
            _player = player;
            _robotHealth = robotTankBrain.GetComponent<Health>();
            _maxDistance = maxDistance;
            _attackProperties = attackProperties;

            InitStateMachine();

            SetupTransitions();  
        }

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

            if (distanceToPlayer <= _attackProperties.Target.MaxDistance)
            {
                _stateMachine.SetState(_robotTargetAttack);
                OnPerformAttack?.Invoke(0);
            }
            else if(distanceToPlayer <= _attackProperties.AoE.JumpDistance)
            {
                _stateMachine.SetState(_robotAreaAttack);
                OnPerformAttack?.Invoke(1);
            }
            else
                _navMeshAgent.destination = _player.transform.position;

        }

        public bool IsDone()
        {
            return _player.Health.Current == 0;
        }

        public bool IsLostPlayer()
        {
            return Vector3.Distance(_player.transform.position, _navMeshAgent.transform.position) > _maxDistance;
        }

        private void InitStateMachine()
        {
            _stateMachine = new StateMachine();

            _robotAreaAttack = new RobotAttack_Area(_navMeshAgent, _player, _robotHealth, _attackProperties.AoE);
            _robotTargetAttack = new RobotAttack_Hammer(_navMeshAgent, _player, _attackProperties.Target);
        }

        private void SetupTransitions()
        {
            _stateMachine.AddTransition(_robotAreaAttack, null, _robotAreaAttack.IsDone);
            _stateMachine.AddTransition(_robotTargetAttack, null, _robotTargetAttack.IsDone);
        }

        [Serializable]
        public class AttacksProperties
        {
            public RobotAttack_Hammer.Properties Target => _targetProperties;
            public RobotAttack_Area.Properties AoE => _aoeProperties;

            [SerializeField] private RobotAttack_Hammer.Properties _targetProperties;
            [SerializeField] private RobotAttack_Area.Properties _aoeProperties;
        }
    }
}
