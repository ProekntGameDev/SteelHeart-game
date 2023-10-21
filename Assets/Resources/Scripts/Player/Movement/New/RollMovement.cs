using UnityEngine;
using Zenject;

public class RollMovement : BaseMovementState
{
    public override bool CanUseCombat() => false;

    public float StaminaCost => _staminaCost;

    [SerializeField] private float _staminaCost;
    [SerializeField] private float _cooldown;
    [SerializeField] private float _distance;
    [SerializeField] private float _speed;

    [Inject] private Player _player;

    private Vector3 _direction;
    private float _currentDistance;
    private float _endTime;

    public override void Enter()
    {
        _player.Stamina.Decay(_staminaCost);

        _currentDistance = 0;

        _direction = _player.transform.forward;
        _direction.y = 0;
        _direction.Normalize();
    }

    public override void Exit()
    {
        _player.Movement.CharacterController.CurrentVelocity = Vector3.zero;

        _endTime = Time.time;
    }

    public override void Tick()
    {
        Vector3 motion = _direction * _speed * Time.fixedDeltaTime;
        _player.Movement.CharacterController.Move(motion);

        _currentDistance += motion.magnitude;
    }

    public bool IsDone() => _currentDistance >= _distance;
    public bool IsReady() => Time.time >= _endTime + _cooldown;
}
