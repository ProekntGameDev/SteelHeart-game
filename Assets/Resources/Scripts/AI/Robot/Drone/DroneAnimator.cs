using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;

public class DroneAnimator : MonoBehaviour
{
    public Animator Animator => _animator;

    [SerializeField, Required] private Health _health;
    [SerializeField, Required] private NavMeshAgent _navMeshAgent;
    [SerializeField, Required] private AI.DroneState_Attack _droneAttackState;
    [SerializeField, Required] private Animator _animator;

    [Header("Animator Parameters")]
    [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Float)] private string _animatorRobotSpeed;
    [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Trigger)] private string _animatorTakeDamage;
    [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Trigger)] private string _animatorDeath;
    [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Trigger)] private string _animatorShoot;

    private void Start()
    {
        _health.OnTakeDamage.AddListener((damage) => _animator.SetTrigger(_animatorTakeDamage));
        _health.OnDeath.AddListener(() => _animator.SetTrigger(_animatorDeath));

        _droneAttackState.OnShoot.AddListener(() => _animator.SetTrigger(_animatorShoot));
    }

    private void Update()
    {
        _animator.SetFloat(_animatorRobotSpeed, _navMeshAgent.velocity.magnitude);
    }
}
