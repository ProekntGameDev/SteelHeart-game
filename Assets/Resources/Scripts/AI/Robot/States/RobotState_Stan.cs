using UnityEngine;

namespace AI
{
    public class RobotState_Stan : IState
    {
        private float _endTime;
        private float _duration;

        public RobotState_Stan(float duration)
        {
            _duration = duration;
        }

        public void OnEnter()
        {
            _endTime = Time.time + _duration;
        }

        public void OnExit()
        {
            _endTime = 0;
        }

        public void Tick()
        { }

        public bool IsDone() => _endTime <= Time.time;
    }
}
