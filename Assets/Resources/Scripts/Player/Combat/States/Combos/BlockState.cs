using UnityEngine;
using UnityEngine.InputSystem;

public class BlockState : BaseCombatState
{
    public override bool IsInterruptible => false;

    public float StanDuration => _stanDuration;

    protected override InputAction[] _buttons => new InputAction[1] { _player.Input.Player.Block };

    [SerializeField] private float _reflectDamageTiming;
    [SerializeField] private float _stanDuration;

    private float _startTime;

    public override void Enter()
    {
        _startTime = Time.time;
    }

    public override void Exit()
    {
        base.Exit();

        _startTime = 0;
    }

    public DamageBlockResult BlockDamage(Damage damage)
    {
        if (_player.Stamina.Current < damage.Value)
        {
            _player.Stamina.Decay(_player.Stamina.Current);
            return DamageBlockResult.Broken;
        }

        _player.Stamina.Decay(damage.Value);

        if (damage.IsPiercing)
            return DamageBlockResult.Broken;

        if (Time.time - _startTime > _reflectDamageTiming)
            return DamageBlockResult.Blocked;
        else
            return DamageBlockResult.Reflected;
    }

    public override bool IsDone() => 
        _player.Input.Player.Block.ReadValue<float>() == 0 ||
        _player.Stamina.Current == 0;
}
