using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class RobotState_Stan : MonoBehaviour, IState
    {
        [SerializeField, Required] private NavMeshAgent _navMeshAgent;
        [SerializeField, Required] private Animator _animator;
        [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Bool)] private string _isInStan;

        private float _duration;
        private float _endTime;

        public void SetDuration(float duration)
        {
            _duration = duration;
        }

        public void OnEnter()
        {
            _endTime = Time.time + _duration;

            _navMeshAgent.isStopped = true;
            _animator.SetBool(_isInStan, true);
        }

        public void OnExit()
        {
            _endTime = 0;

            _navMeshAgent.isStopped = false;
            _animator.SetBool(_isInStan, false);
        }

        public void Tick()
        { }

        public bool IsDone() => _endTime <= Time.time;
    }
}
