using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class RobotState_Patrol : MonoBehaviour, IState
    {
        [SerializeField, Required] private NavMeshAgent _navMeshAgent;
        [SerializeField] private Transform[] _points;
        [SerializeField] private float _speed;

        private int _targetPointIndex;

        public void OnEnter()
        {
            _navMeshAgent.speed = _speed;

            _targetPointIndex++;

            if (_targetPointIndex >= _points.Length)
                _targetPointIndex = 0;

            _navMeshAgent.updateRotation = true;
            _navMeshAgent.SetDestination(_points[_targetPointIndex].position);
        }

        public void OnExit()
        { }

        public void Tick()
        { }

        public bool IsDone()
        {
            return _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance;
        }
    }
}
