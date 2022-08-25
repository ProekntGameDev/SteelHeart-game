using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class JumpController : MonoBehaviour
{
    public float jumpForce;
    public float floorCheckRayLength = 1.05f;
    public int maxJetpackJumps = 0;

    private Rigidbody _rigidbody;
    private bool _isJumpButtonPressedLast;
    private int _currentJetpackJumps;

    public bool IsJumpButtonPressed { get; private set; }
    public bool IsOnFloor { get; private set; }
    public float PreviousVelocityY { get; private set; } 

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _isJumpButtonPressedLast = IsJumpButtonPressed;
        IsJumpButtonPressed = Input.GetAxis("Jump") > 0;
        IsOnFloor = CheckFloor();

        if (IsJumpButtonPressed && _isJumpButtonPressedLast == false)
        {
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

    private void FixedUpdate()
    {
        PreviousVelocityY = _rigidbody.velocity.y;
    }

    public void Jump(float multiplier = 1)
    {
        _rigidbody.velocity += Vector3.up * jumpForce * multiplier;
    }

    private bool CheckFloor()
    {
        return Physics.Raycast(transform.position, Vector3.down, floorCheckRayLength);
    }
}
