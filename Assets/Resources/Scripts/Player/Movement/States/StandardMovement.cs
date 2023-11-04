using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

public class StandardMovement : BaseMovementState
{
    public override bool CanUseCombat() => true;

    public bool IsCrouching { get; private set; }

    [SerializeField, Required, Foldout("Stamina")] private Stamina _stamina;
    [SerializeField, Foldout("Stamina")] private float _idleStaminaRestoration;
    [SerializeField, Foldout("Stamina")] private float _crouchStaminaRestoration;
    [SerializeField, Foldout("Stamina")] private float _walkStaminaRestoration;
    [SerializeField, Foldout("Stamina")] private float _runStaminaConsumption;
    [SerializeField, Foldout("Stamina")] private float _jumpStaminaCost;

    [SerializeField, Foldout("Velocities")] private float _crouchSpeed;
    [SerializeField, Foldout("Velocities")] private float _walkSpeed;
    [SerializeField, Foldout("Velocities")] private float _runSpeed;
    [SerializeField, Foldout("Velocities")] private float _maxAirSpeed;
    [SerializeField, Foldout("Velocities")] private float _airAcceleration;

    [SerializeField] private float _crouchHeight;
    [SerializeField] private float _jumpHeight;

    private float _standHeight;

    private bool _isRunning;
    private bool _wishStandUp;

    private void Start()
    {
        _standHeight = CharacterController.Height;
    }

    public override void Enter()
    { }

    public override void Tick()
    {
        if (_wishStandUp && IsCrouching)
            if (CanStandUp())
                StandUp();

        Vector3 wishDirection = GetWishDirection(CharacterController);
        CharacterController.Rotate(wishDirection);

        if (CharacterController.IsGrounded == false && TryStepOnSlope() == false)
        {
            CharacterController.CurrentVelocity = CharacterController.CurrentVelocity + (wishDirection * _airAcceleration * Time.fixedDeltaTime);

            if (CharacterController.CurrentVelocity.magnitude > _maxAirSpeed)
                CharacterController.CurrentVelocity = CharacterController.CurrentVelocity.normalized * _maxAirSpeed;

            CharacterController.Move();
            return;
        }

        CheckRun();
        float speed = IsCrouching ? _crouchSpeed : (_isRunning ? _runSpeed : _walkSpeed);

        UpdateStamina(wishDirection);

        CharacterController.CurrentVelocity = wishDirection * speed;
        CharacterController.Move();
    }

    public override void Exit()
    { }

    public override void OnJump()
    {
        if (CharacterController.IsGrounded == false || _stamina.Current < _jumpStaminaCost)
            return;

        _stamina.Decay(_jumpStaminaCost);
        CharacterController.VerticalVelocity = Mathf.Sqrt(_jumpHeight * 2 * -CharacterController.Gravity);
    }

    private void CrouchPerformed(InputAction.CallbackContext context)
    {
        _wishStandUp = false;
        IsCrouching = true;
        CharacterController.SetHeight(_crouchHeight);
    }

    private void CrouchCanceled(InputAction.CallbackContext context)
    {
        _wishStandUp = true;

        if (CanStandUp())
            StandUp();
    }

    private void CheckRun()
    { 
        bool isRunPerformed = Player.Input.Player.Run.ReadValue<float>() != 0;

        if (isRunPerformed)
        {
            if (_stamina.Current < _runStaminaConsumption * Time.fixedDeltaTime)
                _isRunning = false;
            else if (_stamina.Current > _runStaminaConsumption)
                _isRunning = true;
        }
        else
            _isRunning = false;
    }

    private void UpdateStamina(Vector3 wishDirection)
    {
        if (wishDirection.sqrMagnitude != 0)
        {
            if (_isRunning)
                _stamina.Decay(_runStaminaConsumption * Time.fixedDeltaTime);
            else if (IsCrouching == false)
                _stamina.RestoreFixedTime(_walkStaminaRestoration);
            else
                _stamina.RestoreFixedTime(_crouchStaminaRestoration);
        }
        else
            _stamina.RestoreFixedTime(_idleStaminaRestoration);
    }

    private void StandUp()
    {
        _wishStandUp = false;
        IsCrouching = false;
        CharacterController.SetHeight(_standHeight);
    }

    private bool TryStepOnSlope()
    {
        float slopeDelta = GetSlopeUnderPlayer();
        CharacterController.Move(slopeDelta * Vector3.down, true);

        return slopeDelta > 0;
    }

    private float GetSlopeUnderPlayer()
    {
        Ray ray = new Ray(CharacterController.transform.position, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, GetSlopeDelta(CharacterController.SlopeLimit)))
        {
            float angle = Vector3.Angle(Vector3.up, raycastHit.normal);
            float slopeDelta = GetSlopeDelta(angle, CharacterController.CurrentVelocity.magnitude);
            if (slopeDelta >= raycastHit.distance)
                return slopeDelta + raycastHit.distance;
        }

        return 0;
    }

    private float GetSlopeDelta(float slopeAngle, float xDelta = 1) => Mathf.Tan(Mathf.Deg2Rad * slopeAngle) * xDelta;

    private bool CanStandUp() => CharacterController.CanSetHeight(_standHeight);

    private void OnEnable()
    {
        Player.Input.Player.Crouch.performed += CrouchPerformed;
        Player.Input.Player.Crouch.canceled += CrouchCanceled;
    }

    private void OnDisable()
    {
        Player.Input.Player.Crouch.performed -= CrouchPerformed;
        Player.Input.Player.Crouch.canceled -= CrouchCanceled;
    }
}
