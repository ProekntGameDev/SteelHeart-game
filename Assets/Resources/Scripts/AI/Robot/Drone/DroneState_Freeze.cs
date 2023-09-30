using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class DroneState_Freeze : IState
    {
        private NavMeshAgent _navMeshAgent;

        private float _minHeight;
        private float _maxHeight;

        private float _duration;
        private float _endTime;

        public DroneState_Freeze
            (NavMeshAgent navMeshAgent, float minHeight, float maxHeight, float duration)
        {
            _navMeshAgent = navMeshAgent;
            _minHeight = minHeight;
            _maxHeight = maxHeight;
            _duration = duration;
        }

        public void OnEnter()
        {
            _endTime = Time.time + _duration;
            _navMeshAgent.baseOffset = _minHeight;
        }

        public void OnExit()
        {
            _endTime = 0;
            _navMeshAgent.baseOffset = _maxHeight;
        }

        public void Tick()
        { }

        public bool IsDone() => _endTime <= Time.time;
    }
}