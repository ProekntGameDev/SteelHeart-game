using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class TankRobotState_Idle : IState
    {
        private Vector2 _delayRange;
        private float? _deadline;
        
        private NavMeshAgent _navMeshAgent;
        private float _stoppingDistance;

        public TankRobotState_Idle(Vector2 delayRange, NavMeshAgent navMeshAgent, float stoppingDistance)
        {
            _delayRange = delayRange;
            _navMeshAgent = navMeshAgent;
            _stoppingDistance = stoppingDistance;
        }

        public void OnEnter()
        {
            _navMeshAgent.stoppingDistance = _stoppingDistance;
            
            float delay = Random.Range(_delayRange.x, _delayRange.y);
            _deadline = delay + Time.time;
        }

        public void OnExit()
        {
            _deadline = null;
        }

        public void Tick()
        {

        }

        public bool IsDone()
        {
            return _deadline.HasValue ? Time.time >= _deadline : false;
        }
    }
}