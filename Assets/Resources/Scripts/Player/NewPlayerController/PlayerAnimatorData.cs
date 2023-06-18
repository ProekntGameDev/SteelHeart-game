using UnityEngine;
using NaughtyAttributes;

public class PlayerAnimatorData : MonoBehaviour, IPlayerAnimator
{
    public Animator Animator => _animator;
    public string IsInAir => _isInAir;
    public string IsCrouching => _isCrouching;
    public string Speed => _speed;
    public string Jump => _jump;

    [Header("Components")]
    [SerializeField, Required] private Animator _animator;

    [Header("Animator Parameters")]
    [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Bool)] private string _isInAir;
    [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Bool)] private string _isCrouching;
    [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Float)] private string _speed;
    [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Trigger)] private string _jump;
}
