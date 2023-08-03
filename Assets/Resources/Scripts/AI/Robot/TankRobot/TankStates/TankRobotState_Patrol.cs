using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class TankRobotState_Patrol : IState
    {
        private float _speed;
        private float _stoppingDistance;
        private NavMeshAgent _navMeshAgent;
        private Transform[] _points;

        private int _targetPointIndex;

        public TankRobotState_Patrol(float speed, float stoppingDistance, NavMeshAgent navMeshAgent, Transform[] points)
        {
            _speed = speed;
            _stoppingDistance = stoppingDistance;
            _navMeshAgent = navMeshAgent;
            _points = points;
        }

        public void OnEnter()
        {
            _navMeshAgent.speed = _speed;
            _navMeshAgent.stoppingDistance = _stoppingDistance;
            
            _targetPointIndex++;

            if (_targetPointIndex >= _points.Length)
                _targetPointIndex = 0;

            _navMeshAgent.SetDestination(_points[_targetPointIndex].position);
        }

        public void OnExit()
        {
        }

        public void Tick()
        {
        }

        public bool IsDone()
        {
            return _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance;
        }
    }
}