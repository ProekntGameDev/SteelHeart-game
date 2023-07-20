using UnityEngine;

public class MoveState : IState
{
    private InertialCharacterController _characterController;
    private float _maxSpeed;
    private float _acceleration;

    public MoveState(InertialCharacterController characterController, float acceleration, float maxSpeed)
    {
        _characterController = characterController;
        _maxSpeed = maxSpeed;
        _acceleration = acceleration;
    }

    public void OnEnter()
    {
    }

    public void OnExit()
    {
    }

    public void Tick()
    {
        Vector3 input = _characterController.LastInput.input;
        Vector3 wishDirection = input.x * Vector3.right + input.z * Vector3.forward;

        _characterController.GroundMove(wishDirection.normalized, _acceleration, _maxSpeed);
        _characterController.Rotate(wishDirection.normalized);
    }
}
