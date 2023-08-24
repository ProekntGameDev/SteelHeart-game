using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using System;

namespace AI
{
    public class RobotState_Combat : IState
    {
        public event Action<int> OnPerformAttack;
        public IReadOnlyList<IRobotAttack> Attacks => _attacks;

        private NavMeshAgent _navMeshAgent;
        private List<IRobotAttack> _attacks;

        private float _maxDistance;
        private Player _player;
        private Health _robotHealth;
        private StateMachine _stateMachine;

        public RobotState_Combat(Player player, HammerRobotBrain robotTankBrain, float maxDistance, List<IRobotAttack> attacks)
        {
            _navMeshAgent = robotTankBrain.GetComponent<NavMeshAgent>();
            _player = player;
            _robotHealth = robotTankBrain.GetComponent<Health>();
            _maxDistance = maxDistance;
            _attacks = attacks;

            InitStateMachine();

            SetupTransitions();  
        }

        public void OnEnter()
        { }

        public void OnExit()
        { }

        public void Tick()
        {
            _stateMachine.Tick();

            if (_stateMachine.HasState)
                return;

            float distanceToPlayer = Vector3.Distance(_player.transform.position, _navMeshAgent.transform.position);
            float playerSpeed = _player.Movement.CharacterController.CurrentVelocity.magnitude;

            IRobotAttack bestAttack = _attacks.Where(x => x.AttackProperties.MaxDistance > distanceToPlayer)
                .Where(x => playerSpeed <= (1 / x.AttackProperties.Speed))
                .OrderByDescending(x => x.AttackProperties.Damage)
                .FirstOrDefault();

            if (bestAttack == null)
                _navMeshAgent.destination = _player.transform.position;
            else
            {
                _stateMachine.SetState(bestAttack);
                OnPerformAttack?.Invoke(_attacks.IndexOf(bestAttack));
            }

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

            foreach (var attack in _attacks)
            {
                attack.Init(_navMeshAgent, _player, _robotHealth);
            }
        }

        private void SetupTransitions()
        {
            foreach (var attack in _attacks)
            {
                _stateMachine.AddTransition(attack, null, () => attack.IsDone());
            }
        }
    }
}
