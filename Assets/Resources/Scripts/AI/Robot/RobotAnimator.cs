using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class RobotAnimator : MonoBehaviour
    {
        public Animator Animator => _animator;

        [SerializeField, Required] private HammerRobotBrain _robotBrain;
        [SerializeField, Required] private Health _health;
        [SerializeField, Required] private NavMeshAgent _navMeshAgent;
        [SerializeField, Required] private AgentLinkMover _agentLinkMover;
        [SerializeField, Required] private Animator _animator;

        [Header("Animator Parameters")]
        [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Float)] private string _animatorRobotSpeed;
        [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Trigger)] private string _animatorStartJump;
        [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Trigger)] private string _animatorEndJump;
        [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Float)] private string _animatorAttackIndex;
        [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Trigger)] private string _animatorAttack;
        [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Trigger)] private string _animatorTakeDamage;
        [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Trigger)] private string _animatorDeath;
        [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Bool)] private string _animatorStan;

        private void Start()
        {
            _health.OnChange.AddListener((value) => _animator.SetTrigger(_animatorTakeDamage));
            _health.OnDeath.AddListener(() => _animator.SetTrigger(_animatorDeath));

            _robotBrain.OnAttack.AddListener(OnAttack);
        }

        private void OnAttack(int index)
        {
            _animator.SetFloat(_animatorAttackIndex, index);
            _animator.SetTrigger(_animatorAttack);
        }

        private void OnEnable()
        {
            _agentLinkMover.OnLinkStart += () => _animator.SetTrigger(_animatorStartJump);
            _agentLinkMover.OnLinkEnd += () => _animator.SetTrigger(_animatorEndJump);
        }

        private void Update()
        {
            _animator.SetFloat(_animatorRobotSpeed, _navMeshAgent.velocity.magnitude);

            _animator.SetBool(_animatorStan, _robotBrain.IsInStan);
        }

        private void OnDisable()
        {
            _agentLinkMover.OnLinkStart -= () => _animator.SetTrigger(_animatorStartJump);
            _agentLinkMover.OnLinkEnd -= () => _animator.SetTrigger(_animatorEndJump);
        }
    }
}
