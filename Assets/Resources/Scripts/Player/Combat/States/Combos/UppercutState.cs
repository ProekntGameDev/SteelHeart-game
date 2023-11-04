using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class UppercutState : BaseCombatState
{
    public override bool IsInterruptible => false;

    protected override InputAction[] _buttons => new InputAction[1] { _player.Input.Player.Fire };

    [SerializeField, Required] private OverlapSphere _attackPoint;
    [SerializeField] private float _height;
    [SerializeField] private float _duration;
    [SerializeField] private Damage _damage;

    [SerializeField] private Animator _animator;
    [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Trigger)] private string _attackTrigger;

    private AIMoveAgent _targetEnemy;
    private float _startTime;

    public override void Enter()
    {
        RotateToClosestEnemy();

        IDamagable targetDamagable = null;
        Collider target = null;

        foreach (var collider in _attackPoint.GetColliders())
        {
            if (collider.TryGetComponent(out IDamagable damagable))
            {
                targetDamagable = damagable;
                target = collider;
            }
        }

        if (target == null)
            return;

        targetDamagable.TakeDamage(_damage, _player.Health);

        if (target.TryGetComponent(out AIMoveAgent agent))
        {
            _targetEnemy = agent;

            float velocity = Mathf.Sqrt(_height * _player.Movement.CharacterController.Gravity * -2);

            _player.Movement.CharacterController.VerticalVelocity = velocity;
            agent.SetVelocity(velocity * Vector3.up);
        }

        _startTime = Time.time;

        _animator.SetTrigger(_attackTrigger);
    }

    public override void Exit()
    {
        base.Exit();

        _targetEnemy = null;
        _startTime = 0;
    }

    public override bool CanEnter() => _player.Movement.CharacterController.IsGrounded == false && _player.Movement.CharacterController.VerticalVelocity > 0;

    public override bool IsDone() => Time.time >= _startTime + _duration;
}
