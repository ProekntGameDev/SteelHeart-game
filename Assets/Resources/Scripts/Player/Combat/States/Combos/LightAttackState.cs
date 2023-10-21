using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

public class LightAttackState : BaseCombatState
{
    public override bool IsInterruptible => false;

    protected override InputAction[] _buttons => new InputAction[1] { _player.Input.Player.Fire };

    [SerializeField, Required] private OverlapSphere _attackPoint;
    [SerializeField] private Damage _damage;

    [SerializeField] private Animator _animator;
    [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Trigger)] private string _attackTrigger;

    public override void Enter()
    {
        RotateToClosestEnemy();

        _animator.SetTrigger(_attackTrigger);
    }

    public override void Exit()
    {
        base.Exit();

        foreach (var collider in _attackPoint.GetColliders())
            if (collider.TryGetComponent(out IDamagable damagable))
                damagable.TakeDamage(_damage, _player.Health);
    }
}
