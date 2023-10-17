using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

public class ParryState : BaseCombatState
{
    public override bool IsInterruptible => false;

    public Health ParryTarget { get; set; }

    protected override InputAction[] _buttons => null;

    [SerializeField, Required] private OverlapSphere _attackPoint;
    [SerializeField] private Damage _damage;
    [SerializeField] private float _maxTime;

    [SerializeField] private Animator _animator;
    [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Trigger)] private string _attackTrigger;

    private bool _wantsCounterattack;
    private float _startTime;

    public override void Enter()
    {
        if (ParryTarget == null)
            throw new System.NullReferenceException(nameof(ParryTarget));

        _startTime = Time.time;
        Time.timeScale = 0.1f;

        _player.Input.Player.Fire.performed += OnFirePerformed;
    }

    public override void Exit()
    {
        base.Exit();

        RotateToTarget(ParryTarget.transform.position);

        if (_wantsCounterattack)
        {
            foreach (var collider in _attackPoint.GetColliders())
                if (collider.TryGetComponent(out IDamagable damagable))
                    damagable.TakeDamage(_damage, _player.Health);
        }

        _wantsCounterattack = false;
        ParryTarget = null;
        _startTime = 0;
        Time.timeScale = 1f;

        _player.Input.Player.Fire.performed -= OnFirePerformed;
    }

    public override void Tick()
    {
        RotateToTarget(ParryTarget.transform.position);
    }

    public override bool IsDone() =>
        _startTime + _maxTime <= Time.time || _wantsCounterattack;

    private void OnFirePerformed(InputAction.CallbackContext context)
    {
        _animator.SetTrigger(_attackTrigger);
        _wantsCounterattack = true;
    }
}
