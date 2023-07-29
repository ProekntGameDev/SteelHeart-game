using UnityEngine;

public class IdleState : IState
{
    private InertialCharacterController _characterController;
    private float _maxVelocity;

    public IdleState(InertialCharacterController characterController, float maxVelocity)
    {
        _characterController = characterController;
        _maxVelocity = maxVelocity;
    }

    public void OnEnter()
    {
    }

    public void OnExit()
    {
    }

    public void Tick()
    {
        _characterController.GroundMove(Vector3.zero, 0, _maxVelocity);
    }
}
