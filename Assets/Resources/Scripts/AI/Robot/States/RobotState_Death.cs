using UnityEngine;

namespace AI
{
    public class RobotState_Death : IState
    {
        private readonly Rigidbody _rigidbody;
        private readonly Animator _animator;
        private readonly float _destroyDelay;

        private GameObject _robot;
        private float _startTime;

        public RobotState_Death(GameObject robot, Rigidbody rigidbody, Animator animator, float destroyDelay)
        {
            _robot = robot;
            _rigidbody = rigidbody;
            _animator = animator;
            _destroyDelay = destroyDelay;
        }

        public RobotState_Death(GameObject robot, float destroyDelay)
        {
            _robot = robot;
            _destroyDelay = destroyDelay;
        }

        public void OnEnter()
        {
            _startTime = Time.time;

            if (_rigidbody != null)
                EnableRagdoll();
        }

        public void OnExit()
        { }

        public void Tick()
        {
            if (IsDone() == false || _robot == null)
                return;

            GameObject.Destroy(_robot);
            _robot = null;
        }

        public bool IsDone() => _startTime + _destroyDelay <= Time.time;

        private void EnableRagdoll()
        {
            _rigidbody.isKinematic = false;
            _animator.enabled = false;
        }
    }
}
