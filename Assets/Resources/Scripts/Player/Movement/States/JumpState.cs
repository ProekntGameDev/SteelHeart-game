using UnityEngine;

public class JumpState : IState
{
    public float StaminaCost => _staminaCost;

    private InertialCharacterController _characterController;
    private Stamina _stamina;
    private JumpType _jumpType;
    private OldMovement.MoveState.Settings _settings;

    private float _staminaCost;
    private float _timestamp;

    public JumpState(InertialCharacterController characterController, Stamina stamina, OldMovement.MoveState.Settings settings, JumpType jumpType, float staminaCost)
    {
        _characterController = characterController;
        _stamina = stamina;
        _staminaCost = staminaCost;
        _jumpType = jumpType;
        _settings = settings;
    }

    public void OnEnter()
    {
        if (_stamina.Current < _settings.StaminCost)
            throw new System.InvalidOperationException();

        _stamina.Decay(_settings.StaminCost);

        _timestamp = Time.time;

        _characterController.UseGravity = false;
        _characterController.VerticalVelocity = Mathf.Sqrt(_jumpType.InitalJumpForce * -2 * _characterController.Gravity);
    }

    public void OnExit()
    {
        _timestamp = 0;

        _characterController.UseGravity = true;
        _characterController.VerticalVelocity = _jumpType.GravityOnRelease;
    }

    public void Tick()
    {
        float gravity = _jumpType.GravityRise.Evaluate(Mathf.Min(Time.time - _timestamp, 1)) * _characterController.Gravity;
        _characterController.VerticalVelocity += gravity * Time.fixedDeltaTime;

        Vector3 input = _characterController.ReadInputAxis();
        Vector3 wishDirection = input.x * Vector3.right + input.y * Vector3.forward;
        wishDirection.Normalize();

        if (_characterController.IsGrounded == false)
            _characterController.AirMove(wishDirection, _settings.Acceleration, _settings.MaxSpeed);
        else
            _characterController.GroundMove(wishDirection, _settings.Acceleration, _settings.MaxSpeed);

        _characterController.Rotate(wishDirection);
    }

    public bool IsDone()
    {
        return _characterController.VerticalVelocity <= 0 || _timestamp == 0;
    }
}
