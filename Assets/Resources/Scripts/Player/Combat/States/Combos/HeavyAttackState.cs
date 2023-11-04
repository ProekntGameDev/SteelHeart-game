using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeavyAttackState : BaseCombatState
{
    public override bool IsInterruptible => true;
    public float MinTime => _minTime;

    protected override InputAction[] _buttons => new InputAction[1] { _player.Input.Player.FireHold };

    [SerializeField] private float _minTime;
    [SerializeField] private float _maxTime;
    [SerializeField] private Damage _minDamage;
    [SerializeField] private Damage _maxDamage;
    [SerializeField, Required] private OverlapSphere _attackPoint;

    [SerializeField] private Animator _animator;
    [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Trigger)] private string _startAttackTrigger;
    [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Trigger)] private string _breakAttackTrigger;
    [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Trigger)] private string _endAttackTrigger;

    private float _startTime;
    private float? _duration;

    public override void Enter()
    {
        RotateToClosestEnemy();

        _animator.SetTrigger(_startAttackTrigger);

        _startTime = Time.time;
        _duration = null;

        _player.Input.Player.Fire.canceled += OnFireCanceled;
    }

    public override void Exit()
    {
        base.Exit();

        if (_duration.HasValue == false)
        {
            _animator.SetTrigger(_breakAttackTrigger);
            return;
        }

        Damage damage = _maxDamage;

        if (_duration < _maxTime)
        {
            float time = Mathf.Clamp01((_duration.Value - _minTime) / (_maxTime - _minTime));
            damage = new Damage(Mathf.Lerp(_minDamage.Value, _maxDamage.Value, time), _minDamage.CanBlock, _minDamage.IsPiercing);
        }

        foreach (var collider in _attackPoint.GetColliders())
            if (collider.TryGetComponent(out IDamagable damagable))
                damagable.TakeDamage(damage, _player.Health);

        _animator.SetTrigger(_endAttackTrigger);

        _player.Combat.ResetCombot();
        _player.Input.Player.Fire.canceled -= OnFireCanceled;
    }

    public override void OnInterrupt()
    {
        _duration = null;
    }

    public override bool IsDone()
    {
        bool result = _duration.HasValue;

        if (Time.time >= _startTime + _maxTime)
        {
            _duration = _maxTime;
            result = true;
        }

        return result;
    }

    public override bool CanEnter() => _player.Movement.CharacterController.IsGrounded;

    private void OnFireCanceled(InputAction.CallbackContext context)
    {
        if (context.canceled == false)
            return;

        _duration = Time.time - _startTime;
    }
}
