using UnityEngine;
using UnityEngine.InputSystem;

public class StanState : BaseCombatState
{
    public override bool IsInterruptible => true;

    protected override InputAction[] _buttons => null;

    private float _duration;
    private float _startTime;

    public void Init(float duration)
    {
        _duration = duration;
    }

    public override void Enter()
    {
        _startTime = Time.time;

        _player.Movement.enabled = false;
    }

    public override void Exit()
    {
        _startTime = 0;

        _player.Movement.enabled = true;
    }

    public override void Tick()
    { }

    public override void OnInterrupt()
    {
        _startTime = 0;
    }

    public override bool IsDone() => Time.time >= _startTime + _duration;

    public override bool IsReady() => true;
}
