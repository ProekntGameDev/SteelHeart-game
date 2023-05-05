using UnityEngine;

[RequireComponent(typeof(AdvancedCharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float Speed { get; private set; }
    public AdvancedCharacterController CharacterController { get; private set; }

    public float RunSpeed => _runSpeed;

    [SerializeField] private float _walkSpeed = 3;
    [SerializeField] private float _runSpeed = 5;
    [SerializeField] private float _crouchSpeed = 2;
    [SerializeField] private float _jumpHeight = 10;
    [SerializeField] private float _speedSmoothTime = 0.1f;

    private float _speedUpdateVelocity;

    private void Awake()
    {
        CharacterController = GetComponent<AdvancedCharacterController>();
    }

    private void Update()
    {
        if (CharacterController.LastInput.jump && CharacterController.IsGrounded)
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        Vector3 direction = CharacterController.LastInput.axis.y * Vector3.back + CharacterController.LastInput.axis.x * Vector3.right;
        direction.Normalize();


        if (direction.sqrMagnitude != 0)
        {
            SetSpeed(CharacterController.LastInput.isCrouching ? _crouchSpeed : (CharacterController.LastInput.isRunning ? _runSpeed : _walkSpeed));
            Rotate(direction);
        }
        else
            SetSpeed(0);

        CharacterController.Move(direction * Speed * Time.deltaTime);
    }

    private void Jump()
    {
        CharacterController.AddYVelocity(_jumpHeight);
    }

    private void SetSpeed(float value)
    {
        Speed = Mathf.SmoothDamp(Speed, value, ref _speedUpdateVelocity, _speedSmoothTime);
    }

    private void Rotate(Vector3 forward)
    {
        transform.localRotation = Quaternion.LookRotation(forward, Vector3.up);
    }
}
