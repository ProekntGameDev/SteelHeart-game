using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class RobotAnimator : MonoBehaviour
    {
        [SerializeField, Required] private NavMeshAgent _navMeshAgent;
        [SerializeField, Required] private AgentLinkMover _agentLinkMover;
        [SerializeField, Required] private Animator _animator;

        [Header("Animator Parameters")]
        [SerializeField, Min(0)] private float _maxWalkSpeed;
        [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Float)] private string _animatorRobotSpeed;
        [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Trigger)] private string _animatorStartJump;
        [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Trigger)] private string _animatorEndJump;
        [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Float)] private string _animatorAttack;

        private void OnEnable()
        {
            _agentLinkMover.OnLinkStart += () => _animator.SetTrigger(_animatorStartJump);
            _agentLinkMover.OnLinkEnd += () => _animator.SetTrigger(_animatorEndJump);
        }

        private void Update()
        {
            _animator.SetFloat(_animatorRobotSpeed, _navMeshAgent.velocity.magnitude);
        }

        private void OnDisable()
        {
            _agentLinkMover.OnLinkStart -= () => _animator.SetTrigger(_animatorStartJump);
            _agentLinkMover.OnLinkEnd -= () => _animator.SetTrigger(_animatorEndJump);
        }
    }
}
