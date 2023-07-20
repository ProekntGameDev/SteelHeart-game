using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public partial class InertialCharacterController
{
    public Vector3 CurrentVelocity { get { return _currentVelocity; } set { _currentVelocity = value; } }
    public float VerticalVelocity { get { return _verticalVelocity; } set { _verticalVelocity = value; } }
    public bool WishJump { get; set; }

    public PlayerInput LastInput { get; private set; }
    public bool IsGrounded { get; private set; }

    public float Gravity => _gravity;

    [SerializeField] private float _gravity = -9.81f;

    private CharacterController _characterController;
    private float _verticalVelocity;
    private Vector3 _currentVelocity;

    public void ApplyGravity()
    {
        if (_verticalVelocity <= 0)
            _verticalVelocity += _gravity * Time.fixedDeltaTime;

        _characterController.Move(new Vector3(0, _verticalVelocity * Time.fixedDeltaTime, 0));

        IsGrounded = _characterController.isGrounded;

        if (IsGrounded && _verticalVelocity < 0)
            _verticalVelocity = 0f;

    }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        LastInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        WishJump |= LastInput.IsJump && IsGrounded;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Vector3 velocityProjection = Vector3.ProjectOnPlane(_currentVelocity, hit.normal);
        velocityProjection.y = 0;
        _currentVelocity = velocityProjection;

        if (Vector3.Angle(Vector3.down, hit.normal) > _characterController.slopeLimit)
            return;

        _verticalVelocity = 0;
    }

    private void OnEnable()
    {
        _characterController.enabled = true;
    }

    private void OnDisable()
    {
        _characterController.enabled = false;
    }
}
