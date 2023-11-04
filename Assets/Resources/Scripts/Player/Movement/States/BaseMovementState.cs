using UnityEngine;
using Zenject;

public abstract class BaseMovementState : MonoBehaviour
{
    public abstract bool CanUseCombat();

    public abstract void Enter();
    public abstract void Tick();
    public abstract void Exit();

    [Inject] protected Player Player { get; private set; }

    protected PlayerCharacterController CharacterController { get; private set; }

    public virtual void OnJump()
    { 
        
    }

    protected Vector3 GetWishDirection(PlayerCharacterController characterController)
    {
        Vector3 input = characterController.ReadInputAxis();

        Vector3 forward = characterController.Forward.normalized;
        Vector3 right = Vector3.Cross(Vector3.up, forward);

        Vector3 wishDirection = input.x * right + input.y * forward;

        wishDirection.Normalize();

        return wishDirection;
    }

    private void Awake()
    {
        CharacterController = Player.Movement.CharacterController;
    }
}
