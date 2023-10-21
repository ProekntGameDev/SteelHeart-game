using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class RobotAnimator : MonoBehaviour
    {
        public Animator Animator => _animator;

        [SerializeField, Required] private Health _health;
        [SerializeField, Required] private NavMeshAgent _navMeshAgent;
        [SerializeField, Required] private AgentLinkMover _agentLinkMover;
        [SerializeField, Required] private Animator _animator;

        [Header("Animator Parameters")]
        [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Float)] private string _animatorRobotSpeed;
        [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Trigger)] private string _animatorStartJump;
        [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Trigger)] private string _animatorEndJump;
        [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Trigger)] private string _animatorTakeDamage;
        [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Bool)] private string _animatorStan;

        private void Start()
        {
            _health.OnTakeDamage.AddListener((damage) => _animator.SetTrigger(_animatorTakeDamage));
        }

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
