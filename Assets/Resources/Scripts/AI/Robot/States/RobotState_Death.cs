using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class RobotState_Death : MonoBehaviour, IState
    {
        [SerializeField, Required] private NavMeshAgent _robot;
        [SerializeField] private float _destroyDelay;
        [SerializeField] private bool _ragdoll;

        [SerializeField, ShowIf(nameof(_ragdoll))] private Rigidbody _rigidbody;

        [SerializeField, HideIf(nameof(_ragdoll))] private Animator _animator;
        [HideIf(nameof(_ragdoll))]
        [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Trigger)] private string _animatorDeathTrigger;

        private float _startTime;

        public void OnEnter()
        {
            _startTime = Time.time;

            if (_ragdoll)
                EnableRagdoll();
            else
                _animator.SetTrigger(_animatorDeathTrigger);
        }

        public void OnExit()
        { }

        public void Tick()
        {
            if (_startTime + _destroyDelay > Time.time || _robot == null)
                return;

            Destroy(_robot.gameObject);
            _robot = null;
        }

        private void EnableRagdoll()
        {
            _rigidbody.isKinematic = false;
            _animator.enabled = false;
        }
    }
}
