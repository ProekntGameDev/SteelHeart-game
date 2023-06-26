using System;
using System.Collections;
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
        private RobotAttack_Area _areaAttack;
        private RobotAttack_Hammer _hammerAttack;

        public RobotState_Combat(Player player, NavMeshAgent navMeshAgent, float maxDistance, Dictionary<Type, RobotAttackProperties> properties)
        {
            _navMeshAgent = navMeshAgent;
            _player = player;
            _maxDistance = maxDistance;

            InitStateMachine(properties);

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
            return false;
        }

        public bool IsLostPlayer()
        {
            return Vector3.Distance(_player.transform.position, _navMeshAgent.transform.position) > _maxDistance;
        }

        private void InitStateMachine(Dictionary<Type, RobotAttackProperties> properties)
        {
            _stateMachine = new StateMachine();

            Health playerHealth = _player.GetComponent<Health>();

            _areaAttack = new RobotAttack_Area(_navMeshAgent, properties[typeof(RobotAttack_Area)]);
            _hammerAttack = new RobotAttack_Hammer(playerHealth, _navMeshAgent, properties[typeof(RobotAttack_Hammer)]);

            _attacks = new List<IRobotAttack> 
            { 
                _areaAttack, _hammerAttack
            };
        }

        private void SetupTransitions()
        {
            _stateMachine.AddTransition(_hammerAttack, null, () => _hammerAttack.IsDone());
            _stateMachine.AddTransition(_areaAttack, null, () => _areaAttack.IsDone());
        }
    }
}
