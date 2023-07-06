using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class AdvancedCharacterController : MonoBehaviour
{
    public bool IsGrounded { get; private set; }
    public CharacterControllerInput LastInput { get; private set; }

    [SerializeField] private Transform _groundRay;
    [SerializeField] private float _groundRayLength = 0.2f;
    [SerializeField] private float _gravity = -9.81f;

    private float _yVelocity;
    private CharacterController _characterController;

    public CollisionFlags Move(Vector3 motion) => _characterController.Move(motion);

    public void AddYVelocity(float value)
    {
        if (value <= 0)
            throw new System.ArgumentOutOfRangeException(nameof(value));

        _yVelocity += value;
    }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        ApplyGravity();
        HandleInput();
        IsGrounded &= CheckGrounded();
    }

    private void HandleInput()
    {
        Vector2 axis = new Vector2(Input.GetAxis("Horizontal"), -Input.GetAxis("Vertical"));
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        bool isCrouching = Input.GetKey(KeyCode.LeftControl);
        bool jump = Input.GetKeyDown(KeyCode.Space);

        LastInput = new CharacterControllerInput(axis, isRunning, isCrouching, jump);
    }

    private void ApplyGravity()
    {
        _yVelocity -= _gravity * Time.deltaTime;

        _characterController.Move(new Vector3(0, _yVelocity * Time.deltaTime, 0));
    }

    private bool CheckGrounded()
    {
        if (Physics.Raycast(new Ray(_groundRay.position, Vector3.down), out RaycastHit raycastHit, _groundRayLength))
            if (Vector3.Angle(Vector3.up, raycastHit.normal) <= _characterController.slopeLimit)
                if (_yVelocity < _groundRayLength)
                    return true;

        return false;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (Vector3.Angle(Vector3.up, hit.normal) > _characterController.slopeLimit)
            return;

        IsGrounded = true;
        _yVelocity = _gravity * Time.deltaTime * Time.deltaTime;
    }

    public struct CharacterControllerInput
    {
        public readonly Vector2 axis;
        public readonly bool isRunning;
        public readonly bool isCrouching;
        public readonly bool jump;

        public CharacterControllerInput(Vector2 axis, bool isRunning, bool isCrouching, bool jump)
        {
            this.axis = axis;
            this.isRunning = isRunning;
            this.isCrouching = isCrouching;
            this.jump = jump;
        }
    }
}
