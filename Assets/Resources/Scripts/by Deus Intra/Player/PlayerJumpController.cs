using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerMovementController))]
public class PlayerJumpController : MonoBehaviour
{
    public float jumpForce = 5f;
    public int maxJetpackJumps = 0;
    public float jumpTimeout = 0.1f;
    [Space]
    public KeyCode jumpButton = KeyCode.Space;

    private Rigidbody _rigidbody;
    private PlayerMovementController _movementController;
    private float _currentJumpTimeout;
    private int _currentJetpackJumps;

    public bool IsJumpButtonDown { get; private set; }
    public bool IsJumpButtonPressed { get; private set; }
    public float PreviousVelocityY { get; private set; } 


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _movementController = GetComponent<PlayerMovementController>();
    }

    private void Update()
    {
        _currentJumpTimeout -= Time.deltaTime;
        IsJumpButtonDown = Input.GetKeyDown(jumpButton);
        IsJumpButtonPressed = Input.GetKey(jumpButton);
        if (IsJumpButtonDown)
        {
            _currentJumpTimeout = jumpTimeout;
        }
    }

    private void FixedUpdate()
    {
        PreviousVelocityY = _rigidbody.velocity.y;

        if (_currentJumpTimeout > 0)
        {
            bool IsOnFloor = _movementController.IsOnFloor;
            if (IsOnFloor)
            {
                _currentJetpackJumps = maxJetpackJumps;
                Jump();
            }
            else if (_currentJetpackJumps > 0)
            {
                _currentJetpackJumps--;
                Jump();
            }
        }
    }

    public void Jump(float multiplier = 1)
    {
        _rigidbody.velocity += Vector3.up * jumpForce * multiplier;
        _currentJumpTimeout = 0;
    }
}
