using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class TankRobotState_Idle : IState
    {
        private Vector2 _delayRange;
        private float? _deadline;

        public TankRobotState_Idle(Vector2 delayRange)
        {
            _delayRange = delayRange;
        }

        public void OnEnter()
        {
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