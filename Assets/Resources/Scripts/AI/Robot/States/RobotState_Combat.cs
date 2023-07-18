using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace AI
{
    public class RobotState_Combat : IState
    {
        private NavMeshAgent _navMeshAgent;
        private List<IRobotAttack> _attacks;

        private float _maxDistance;
        private Player _player;
        private StateMachine _stateMachine;

        public RobotState_Combat(Player player, NavMeshAgent navMeshAgent, float maxDistance, List<IRobotAttack> attacks)
        {
            _navMeshAgent = navMeshAgent;
            _player = player;
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

            float distance = Vector3.Distance(_player.transform.position, _navMeshAgent.transform.position);

            IRobotAttack bestAttack = null;

            foreach (var e in _attacks)
            {
                if (bestAttack == null)
                {
                    if (e.AttackProperties.MaxDistance < distance)
                        continue;

                    bestAttack = e;
                    continue;
                }

                if (e.AttackProperties.MaxDistance < bestAttack.AttackProperties.MaxDistance)
                    bestAttack = e;
            }

            if (bestAttack == null)
                _navMeshAgent.destination = _player.transform.position;
            else
                _stateMachine.SetState(bestAttack);

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

            Health playerHealth = _player.Health;

            foreach (var attack in _attacks)
            {
                attack.Init(_navMeshAgent, playerHealth);
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
