using UnityEngine;

public class JumpState : IState
{
    private InertialCharacterController _characterController;
    private JumpType _jumpType;

    private float _airAcceleration;
    private float _maxVelocity;
    private float _timestamp;

    public JumpState(InertialCharacterController characterController, float airAcceleration, float maxVelocity, JumpType jumpType)
    {
        _characterController = characterController;
        _jumpType = jumpType;
        _airAcceleration = airAcceleration;
        _maxVelocity = maxVelocity;
    }

    public void OnEnter()
    {
        _timestamp = Time.time;

        _characterController.VerticalVelocity = Mathf.Sqrt(_jumpType.InitalJumpForce * -2 * _characterController.Gravity);

        _characterController.WishJump = false;
    }

    public void OnExit()
    {
        _timestamp = 0;

        _characterController.VerticalVelocity -= _jumpType.GravityOnRelease;
    }

    public void Tick()
    {
        if (_timestamp == 0)
            return;

        float gravity = _jumpType.GravityRise.Evaluate(Mathf.Min(Time.time - _timestamp, 1)) * _characterController.Gravity;
        _characterController.VerticalVelocity += gravity * Time.fixedDeltaTime;

        Vector3 input = _characterController.LastInput.input;
        Vector3 wishDirection = input.x * Vector3.right + input.z * Vector3.forward;
        wishDirection.Normalize();

        if(_characterController.IsGrounded == false)
            _characterController.AirMove(wishDirection, _airAcceleration, _maxVelocity);
        else
            _characterController.GroundMove(wishDirection, _airAcceleration, _maxVelocity);

        _characterController.Rotate(wishDirection);
    }

    public bool IsDone()
    {
        return _characterController.VerticalVelocity <= 0 || _timestamp == 0;
    }
}
