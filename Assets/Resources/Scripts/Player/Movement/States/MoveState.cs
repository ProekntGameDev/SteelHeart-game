using UnityEngine;

public class MoveState : IState
{
    protected InertialCharacterController _characterController { get; private set; }
    private float _maxSpeed;
    private float _acceleration;

    public MoveState(InertialCharacterController characterController, float acceleration, float maxSpeed)
    {
        _characterController = characterController;
        _maxSpeed = maxSpeed;
        _acceleration = acceleration;
    }

    public virtual void OnEnter()
    {
    }

    public virtual void OnExit()
    {
    }

    public void Tick()
    {
        Vector3 input = _characterController.ReadInputAxis();
        Vector3 wishDirection = input.x * Vector3.right + input.y * Vector3.forward;

        wishDirection.Normalize();

        Move(wishDirection, _acceleration, _maxSpeed);
    }

    protected virtual void Move(Vector3 wishDirection, float acceleration, float maxSpeed)
    {
        _characterController.GroundMove(wishDirection, acceleration, maxSpeed);
        _characterController.Rotate(wishDirection);
    }
}
