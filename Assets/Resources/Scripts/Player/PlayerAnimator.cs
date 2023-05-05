using NaughtyAttributes;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [Header("Components")]
    [SerializeField, Required] private Animator _animator;
    [SerializeField, Required] private PlayerMovement _playerMovement;

    [Header("Animator Parameters")]
    [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Bool)] private string _isInAir;
    [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Bool)] private string _isCrouching;
    [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Float)] private string _speed;
    [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Trigger)] private string _jump;

    private void Update()
    {
        _animator.SetBool(_isInAir, !_playerMovement.CharacterController.IsGrounded);
        _animator.SetBool(_isCrouching, _playerMovement.CharacterController.LastInput.isCrouching);
        _animator.SetFloat(_speed, _playerMovement.Speed / _playerMovement.RunSpeed);

        if(_playerMovement.CharacterController.LastInput.jump)
            _animator.SetTrigger(_jump);
    }
}
