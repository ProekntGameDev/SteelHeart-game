using UnityEngine;

public class AirMoveState : OldMovement.MoveState
{
    public AirMoveState(InertialCharacterController characterController, Stamina stamina, Settings settings) : base (characterController, stamina, settings)
    { }

    protected override void Move(Vector3 wishDirection, float acceleration, float maxSpeed)
    {
        _characterController.AirMove(wishDirection, acceleration, maxSpeed);
        _characterController.Rotate(wishDirection);
    }
}
