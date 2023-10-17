public class CrouchMoveState : OldMovement.MoveState
{
    public float StandHeight => _standHeight;

    private float _standHeight;
    private float _crouchHeight;

    public CrouchMoveState(InertialCharacterController characterController, Stamina stamina, float crouchHeight, Settings settings) : base(characterController, stamina, settings)
    {
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

    public bool CanExit() => _characterController.CanSetHeight(_standHeight);
}
