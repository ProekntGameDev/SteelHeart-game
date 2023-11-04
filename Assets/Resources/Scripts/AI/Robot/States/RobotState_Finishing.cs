using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace AI
{
    public class RobotState_Finishing : MonoBehaviour, IState, IFinishable
    {
        public UnityEvent OnStartFinish;
        public UnityEvent OnEndFinish;

        public FinishAnimation FinishAnimation => _finishAnimation;

        [SerializeField, Required] private NavMeshAgent _navMeshAgent;
        [SerializeField, Required] private HammerRobotBrain _robotBrain;
        [SerializeField, Required] private Animator _animator;
        [SerializeField] private FinishAnimation _finishAnimation;
        [SerializeField] private float _duration;

        private bool _isFinishing;
        private float _endTime;

        public void OnEnter()
        {
            _endTime = Time.time + _duration;

            _navMeshAgent.isStopped = true;
            _navMeshAgent.enabled = false;

            _animator.Play(_finishAnimation.EnemyState);

            OnStartFinish?.Invoke();
        }

        public void OnExit()
        {
            _endTime = 0;

            if (_isFinishing)
                Destroy(_navMeshAgent.gameObject);
            else
                OnEndFinish?.Invoke();
        }

        public void Tick()
        { }

        public bool IsDone() => _endTime <= Time.time;

        public bool TryFinish()
        {
            if (_endTime == 0 || _robotBrain.IsInDeathState)
                return false;

            _endTime = Time.time + _finishAnimation.Duration;
            _isFinishing = true;
            OnEndFinish?.Invoke();

            return true;
        }
    }
}
