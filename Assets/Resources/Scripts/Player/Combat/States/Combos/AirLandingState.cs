using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

public class AirLandingState : BaseCombatState
{
    public override bool IsInterruptible => false;

    protected override InputAction[] _buttons => new InputAction[1] { _player.Input.Player.FireHold };

    [SerializeField, Required] private OverlapSphere _attackPoint;
    [SerializeField] private float _additionalVerticalSpeed;
    [SerializeField] private Damage _damage;

    [SerializeField] private Animator _animator;
    [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Trigger)] private string _startAttackTrigger;
    [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Trigger)] private string _landTrigger;

    public override void Enter()
    {
        _animator.SetTrigger(_startAttackTrigger);
    }

    public override void Tick()
    {
        // Double gravity for more impact
        _player.Movement.CharacterController.VerticalVelocity += -_additionalVerticalSpeed * Time.fixedDeltaTime;
    }

    public override void Exit()
    {
        base.Exit();

        foreach (var collider in _attackPoint.GetColliders())
            if (collider.TryGetComponent(out IDamagable damagable) && damagable != (IDamagable)_player.Health)
                damagable.TakeDamage(_damage, _player.Health);

        _animator.SetTrigger(_landTrigger);
    }

    public override bool CanEnter() => _player.Movement.CharacterController.IsGrounded == false && _player.Movement.CharacterController.VerticalVelocity <= 0;

    public override bool IsDone() => _player.Movement.CharacterController.IsGrounded;
}
