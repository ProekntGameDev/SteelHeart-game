using UnityEngine;
using Zenject;

public class SlideMovement : BaseMovementState
{
    public override bool CanUseCombat() => false;

    public float StaminaCost => _staminaCost;

    [SerializeField] private float _staminaCost;
    [SerializeField] private float _cooldown;
    [SerializeField] private float _impulse;
    [SerializeField] private float _friction;
    [SerializeField] private float _minSpeed;

    [Inject] private Player _player;

    private float _endTime;
    private Vector3 _velocity;

    public override void Enter()
    {
        Vector3 direction = _player.Movement.CharacterController.CurrentVelocity.normalized;

        _velocity = _player.Movement.CharacterController.CurrentVelocity + direction * _impulse;
        _velocity.y = 0;
    }

    public override void Exit()
    {
        _velocity = Vector3.zero;

        _endTime = Time.time;
    }

    public override void Tick()
    {
        float speed = _velocity.magnitude;
        float drop = speed * _friction * Time.deltaTime;
        _velocity *= Mathf.Max(speed - drop, 0f) / speed;


        Vector3 motion = _velocity * Time.fixedDeltaTime;
        _player.Movement.CharacterController.Move(motion);
    }

    public bool IsDone() => _velocity.magnitude <= _minSpeed;
    public bool IsReady() => Time.time >= _endTime + _cooldown && _player.Movement.CharacterController.CurrentVelocity.magnitude >= _minSpeed;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        _velocity = Vector3.ProjectOnPlane(_velocity, hit.normal);
    }
}
