using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class RobotState_Stan : IState
    {
        private NavMeshAgent _navMeshAgent;
        private float _duration;

        private float _endTime;

        public RobotState_Stan(NavMeshAgent navMeshAgent, float duration)
        {
            _navMeshAgent = navMeshAgent;
            _duration = duration;
        }

        public void OnEnter()
        {
            _endTime = Time.time + _duration;

            _navMeshAgent.isStopped = true;
        }

        public void OnExit()
        {
            _endTime = 0;

            _navMeshAgent.isStopped = false;
        }

        public void Tick()
        { }

        public bool IsDone() => _endTime <= Time.time;
    }
}
