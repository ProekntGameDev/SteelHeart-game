using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class DroneState_Freeze : IState
    {
        private NavMeshAgent _navMeshAgent;
        private DroneState_Attack _attackState;

        private float _minHeight;
        private float _duration;

        private float _maxHeight;
        private float _endTime;

        public DroneState_Freeze
            (NavMeshAgent navMeshAgent, DroneState_Attack attackState, float minHeight, float duration)
        {
            _navMeshAgent = navMeshAgent;
            _attackState = attackState;

            _minHeight = minHeight;
            _duration = duration;
        }

        public void OnEnter()
        {
            _maxHeight = _navMeshAgent.baseOffset;

            _endTime = Time.time + _duration;

            _navMeshAgent.baseOffset = _minHeight;
            _navMeshAgent.isStopped = true;
        }

        public void OnExit()
        {
            _endTime = 0;

            _attackState.Reload();

            _navMeshAgent.baseOffset = _maxHeight;
            _navMeshAgent.isStopped = false;
        }

        public void Tick()
        { }

        public bool IsDone() => _endTime <= Time.time;
    }
}