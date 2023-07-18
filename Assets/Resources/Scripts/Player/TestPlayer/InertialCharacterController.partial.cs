using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public partial class InertialCharacterController
{
    public Vector3 CurrentVelocity { get { return _currentVelocity; } set { _currentVelocity = value; } }
    public float VerticalVelocity { get { return _verticalVelocity; } set { _verticalVelocity = value; } }

    public CharacterControllerInput LastInput { get; private set; }

    public bool IsGrounded => _characterController.isGrounded;

    public float Gravity => _gravity;

    [SerializeField] private float _gravity = -9.81f;

    private CharacterController _characterController;
    private float _verticalVelocity;
    private Vector3 _currentVelocity;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        LastInput = new CharacterControllerInput();
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        bool isJump = Input.GetKeyDown(KeyCode.Space);
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        bool isCrouching = Input.GetKey(KeyCode.LeftControl);

        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), Input.GetKey(KeyCode.Space) ? 1 : 0, -Input.GetAxis("Vertical"));

        LastInput.Update(input, isRunning, isCrouching, isJump);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (Vector3.Angle(Vector3.down, hit.normal) > _characterController.slopeLimit)
            return;

        _verticalVelocity = 0;

        Vector3 fullVelocity = new Vector3(CurrentVelocity.x, _verticalVelocity, CurrentVelocity.z);

        Vector3 velocity = Vector3.ProjectOnPlane(fullVelocity, hit.normal);

        _currentVelocity = new Vector3(velocity.x, 0, velocity.z);
        _verticalVelocity = velocity.y;
    }

    public class CharacterControllerInput
    {
        public bool WishJump { get; set; }

        public Vector3 input { get; private set; }
        public bool isRunning { get; private set; }
        public bool isCrouching { get; private set; }

        public void Update(Vector3 input, bool isRunning, bool isCrouching, bool jump)
        {
            this.input = input;
            this.isRunning = isRunning;
            this.isCrouching = isCrouching;

            WishJump |= jump;
        }
    }
}
