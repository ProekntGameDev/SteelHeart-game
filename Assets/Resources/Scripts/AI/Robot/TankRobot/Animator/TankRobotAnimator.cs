using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class TankRobotAnimator : MonoBehaviour
    {
        [SerializeField, Required] private NavMeshAgent _navMeshAgent;
        [SerializeField, Required] private Animator _animator;

        [Header("Animator Parameters")]
        [SerializeField, Min(0)] private float _maxWalkSpeed;
        [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Float)] private string _animatorRobotSpeed;
        [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Float)] private string _animatorAttack;

        private void Update()
        {
            _animator.SetFloat(_animatorRobotSpeed, _navMeshAgent.velocity.magnitude);
        }

    }
}