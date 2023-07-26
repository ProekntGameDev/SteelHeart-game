using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class RobotState_Chase : IState
    {
        private RobotVision _robotVision;
        private NavMeshAgent _navMeshAgent;
        private Player _player;
        private float _speed;
        private float _minDistance;
        private float _maxDistance;

        public RobotState_Chase(RobotVision robotVision, NavMeshAgent navMeshAgent, float speed, float minDistance, float maxDistance)
        {
            _robotVision = robotVision;
            _navMeshAgent = navMeshAgent;
            _speed = speed;
            _minDistance = minDistance;
            _maxDistance = maxDistance;
        }

        public void OnEnter()
        {
            _navMeshAgent.speed = _speed;
            _navMeshAgent.stoppingDistance = _minDistance;
        }

        public void OnExit()
        {
            _player = null;
            _navMeshAgent.stoppingDistance = 0;
        }

        public void Tick()
        {
            if (_robotVision.IsVisible(out _player))
            {
                // bool outOfRange = _navMeshAgent.remainingDistance > _navMeshAgent.stoppingDistance;
                // if (!outOfRange)
                // {
                //     _navMeshAgent.isStopped = true;
                // }
                // else
                // {
                //     _navMeshAgent.isStopped = false;
                //     _navMeshAgent.destination = _player.transform.position;
                // }
                _navMeshAgent.destination = _player.transform.position;
            }
                
        }

        public bool IsLostPlayer()
        {
            return _player == null || Vector3.Distance(_player.transform.position, _navMeshAgent.transform.position) > _maxDistance;
        }

        public bool IsDone()
        {
            return _navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete && _player != null;
        }
    }
}
