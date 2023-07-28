using UnityEngine;
using NaughtyAttributes;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Animator _animator;

    [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Float)] string _playerSpeed;
    [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Bool)] string _isGrounded;
    [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Trigger)] string _jump;

    private void Start()
    {
        _player.Movement.OnJump.AddListener(() => _animator.SetTrigger(_jump));
    }

    private void Update()
    {
        _animator.SetFloat(_playerSpeed, _player.Movement.CharacterController.CurrentVelocity.magnitude);
        _animator.SetBool(_isGrounded, _player.Movement.CharacterController.IsGrounded);
    }
}
