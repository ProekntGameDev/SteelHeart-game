using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Stamina))]
public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 250;
    public float sprintSpeed = 500;
    public float sprintStaminaSpend = 2f;
    public float floorCheckRayLength = 1.05f;
    [Space]
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode climbUpwardsKey = KeyCode.W;
    public KeyCode climbDownwardsKey = KeyCode.S;

    public bool IsOnFloor { get; private set; } = false;

    [HideInInspector] public bool isWalkingAllowed;

    private float _horizontalAxis;
    private Rigidbody _rigidbody;
    private Stamina _stamina;

    private bool _isSprinting = false;
    private bool _isSprintKeyPressed;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _stamina = GetComponent<Stamina>();
        isWalkingAllowed = true;
    }
    private void Update()
    {
        _horizontalAxis = Input.GetAxis("Horizontal");
        _isSprintKeyPressed = Input.GetKey(sprintKey);
    }

    private void FixedUpdate()
    {
        IsOnFloor = CheckFloor();
        _isSprinting = (_isSprintKeyPressed && _stamina.IsSufficient) || _isSprinting;

        if (isWalkingAllowed && _isSprinting == false)
        {
            Walk(walkSpeed);
        }
        else if (_isSprintKeyPressed && _isSprinting && _stamina.Current > 0)
        {
            Walk(sprintSpeed);
            _stamina.DecayFixedTime(sprintStaminaSpend);
        }
        else
        {
            _isSprinting = false;
            _stamina.RestoreFixedTime();
        }        
    }

    private void Walk(float speed)
    {
        Vector3 velocity = Vector3.right * speed * _horizontalAxis * Time.fixedDeltaTime;
        //_rigidbody.AddForce(velocity, ForceMode.VelocityChange);
        velocity.y = _rigidbody.velocity.y;
        _rigidbody.velocity = velocity;
    }

    private bool CheckFloor()
    {
        return Physics.Raycast(transform.position + Vector3.up, Vector3.down, floorCheckRayLength);
    }
}
