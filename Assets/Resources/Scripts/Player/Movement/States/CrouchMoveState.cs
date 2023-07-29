public class CrouchMoveState : MoveState
{
    private InertialCharacterController _characterController;
    private float _standHeight;
    private float _crouchHeight;

    public CrouchMoveState(InertialCharacterController characterController, float crouchHeight, float acceleration, float maxSpeed) : base(characterController, acceleration, maxSpeed)
    {
        _characterController = characterController;
        _crouchHeight = crouchHeight;
    }

    public override void OnEnter()
    {
        _standHeight = _characterController.Height;
        _characterController.SetHeight(_crouchHeight);
    }

    public override void OnExit()
    {
        _characterController.SetHeight(_standHeight);
        _standHeight = 0;
    }
}
