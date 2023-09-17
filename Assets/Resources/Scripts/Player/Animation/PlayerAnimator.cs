using UnityEngine;
using NaughtyAttributes;
using Zenject;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _ragdoll;

    [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Float)] string _playerSpeed;
    [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Float)] string _crouch;
    [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Bool)] string _isGrounded;
    [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Bool)] string _isOnLadder;
    [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Trigger)] string _jump;
    [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Trigger)] string _attack;

    [Inject] private Player _player;

    private void Start()
    {
        _player.Movement.OnJump.AddListener(() => _animator.SetTrigger(_jump));

        _player.Combat.OnAttack.AddListener(() => _animator.SetTrigger(_attack));
    }

    private void Update()
    {
        _animator.SetFloat(_crouch, _player.Movement.IsInCrouch ? 1 : 0);
        _animator.SetBool(_isOnLadder, _player.Movement.Ladder);
        _animator.SetFloat(_playerSpeed, _player.Movement.CharacterController.CurrentVelocity.magnitude);
        _animator.SetBool(_isGrounded, _player.Movement.CharacterController.IsGrounded);
    }

    private void OnEnable()
    {
        _animator.enabled = true;
        _ragdoll.isKinematic = true;
    }

    private void OnDisable()
    {
        _animator.enabled = false;
        _ragdoll.isKinematic = false;
    }
}
