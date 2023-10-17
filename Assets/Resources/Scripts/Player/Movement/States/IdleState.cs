using UnityEngine;

public class IdleState : IState
{
    private InertialCharacterController _characterController;
    private float _staminRestorationMultiplier;
    private Stamina _stamina;
    private float _maxVelocity;

    public IdleState(InertialCharacterController characterController, Stamina stamina, float maxVelocity, float staminRestorationMultiplier)
    {
        _characterController = characterController;
        _staminRestorationMultiplier = staminRestorationMultiplier;
        _stamina = stamina;
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
        _stamina.RestoreFixedTime(_staminRestorationMultiplier);
        _characterController.GroundMove(Vector3.zero, 0, _maxVelocity);
    }
}
