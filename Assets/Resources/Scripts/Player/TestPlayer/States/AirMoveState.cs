using UnityEngine;

public class AirMoveState : MoveState
{
    private InertialCharacterController _characterController;

    public AirMoveState(InertialCharacterController characterController, float acceleration, float maxSpeed) : base (characterController, acceleration, maxSpeed)
    {
        _characterController = characterController;
    }

    protected override void Move(Vector3 wishDirection, float acceleration, float maxSpeed)
    {
        _characterController.AirMove(wishDirection, acceleration, maxSpeed);
        _characterController.Rotate(wishDirection);
    }
}
