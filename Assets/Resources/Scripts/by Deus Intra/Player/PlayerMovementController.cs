using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Stamina))]
public class PlayerMovementController : MonoBehaviour
{
    public float walkSpeed = 200;
    public float maxWalkSpeed = 1;
    public float maxSpeedMultiplier = 2f;
    public float accelerationMultiplier = 2f;
    public float sprintStaminaSpend = 2f;
    public float floorCheckRayLength = 1.05f;
    [Space]
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode climbUpwardsKey = KeyCode.W;
    public KeyCode climbDownwardsKey = KeyCode.S;


    public bool IsOnFloor { get; private set; } = false;

    [HideInInspector] public bool isWalkingBanned = false;

    private float MoveControl_HorizontalAxis;
    private bool _isSprinting = false;
    private Rigidbody _rigidbody;
    private Stamina _stamina;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _stamina = GetComponent<Stamina>();
    }

    private void Update()
    {
        MoveControl_HorizontalAxis = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        IsOnFloor = CheckFloor();
        if (isWalkingBanned == false)
            Walk(walkSpeed * MoveControl_HorizontalAxis);

        bool isSprintButtonPressed = Input.GetKey(sprintKey);
        bool isSprintButtonDown = Input.GetKeyDown(sprintKey);
        if (isSprintButtonDown && _stamina.IsSufficient)
        {
            _isSprinting = true;
            maxWalkSpeed *= maxSpeedMultiplier;
            walkSpeed *= accelerationMultiplier;
        }
        else if (isSprintButtonPressed && _isSprinting)
        {
            if (_stamina.Current > 0)
            {
                _stamina.Decay(sprintStaminaSpend * Time.fixedDeltaTime);
            }
            else _isSprinting = false;
        }
        else
        {
            _stamina.RestoreFixedTime();
        }
    }

    private void Walk(float speed)
    {        
        _rigidbody.AddForce(Vector3.right * speed, ForceMode.Acceleration);

        float direction = (_rigidbody.velocity.x >= 0) ? 1 : -1;
        if (Mathf.Abs(_rigidbody.velocity.x) > maxWalkSpeed)
            _rigidbody.velocity = _rigidbody.velocity + Vector3.right * (maxWalkSpeed * direction - _rigidbody.velocity.x);
    }

    private bool CheckFloor()
    {
        return Physics.Raycast(transform.position + Vector3.up, Vector3.down, floorCheckRayLength);
    }
}
