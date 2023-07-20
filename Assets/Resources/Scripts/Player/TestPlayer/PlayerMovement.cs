using UnityEngine;

[RequireComponent(typeof(InertialCharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public InertialCharacterController CharacterController { get; private set; }

    [SerializeField] private float _maxCrouchVelocity = 4;
    [SerializeField] private float _maxWalkVelocity = 6;
    [SerializeField] private float _maxRunVelocity = 10;
    [SerializeField] private float _maxAirVelocity = 100;
    [SerializeField] private float _groundAcceleration = 150;
    [SerializeField] private float _airAcceleration = 1500;
    [SerializeField] private JumpType _jumpType;

    private StateMachine _stateMachine;

    private IdleState _idleState;
    private MoveState _walkState;
    private MoveState _runState;
    private MoveState _crouchState;
    private AirMoveState _airMoveState;
    private JumpState _jumpState;

    private void Awake()
    {
        CharacterController = GetComponent<InertialCharacterController>();

        InitStateMachine();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_stateMachine == null)
            return;

        InitStateMachine();
    }
#endif

    private void InitStateMachine()
    {
        _stateMachine = new StateMachine();

        SetupStates();
        SetupTransitions();

        _stateMachine.SetState(_idleState);
    }

    private void SetupStates()
    {
        _idleState = new IdleState(CharacterController, _maxWalkVelocity);
        _walkState = new MoveState(CharacterController, _groundAcceleration, _maxWalkVelocity);
        _runState = new MoveState(CharacterController, _groundAcceleration, _maxRunVelocity);
        _crouchState = new MoveState(CharacterController, _groundAcceleration, _maxCrouchVelocity);
        _airMoveState = new AirMoveState(CharacterController, _airAcceleration, _maxAirVelocity);
        _jumpState = new JumpState(CharacterController, _airAcceleration, _maxAirVelocity, _jumpType);
    }

    private void SetupTransitions()
    {
        // Base movement

        _stateMachine.AddTransition(_idleState, _walkState, () => CharacterController.LastInput.input.sqrMagnitude != 0);
        _stateMachine.AddTransition(_walkState, _idleState, () => CharacterController.LastInput.input.sqrMagnitude == 0);

        _stateMachine.AddTransition(_walkState, _crouchState, () => CharacterController.LastInput.IsCrouching);
        _stateMachine.AddTransition(_walkState, _runState, () => CharacterController.LastInput.IsRunning);

        _stateMachine.AddTransition(_crouchState, _walkState, () => CharacterController.LastInput.IsCrouching == false);

        _stateMachine.AddTransition(_runState, _walkState, () => CharacterController.LastInput.IsRunning == false);

        // Jump and air movement

        _stateMachine.AddAnyTransition(_airMoveState, () => CharacterController.IsGrounded == false && _jumpState.IsDone());
        _stateMachine.AddAnyTransition(_jumpState, () => CharacterController.IsGrounded && CharacterController.WishJump);

        _stateMachine.AddTransition(_jumpState, _idleState, () => CharacterController.IsGrounded);
        _stateMachine.AddTransition(_airMoveState, _idleState, () => CharacterController.IsGrounded);
    }

    private void FixedUpdate()
    {
        CharacterController.ApplyGravity();

        _stateMachine.Tick();    
    }

    private void OnEnable()
    {
        CharacterController.enabled = true;
    }

    private void OnDisable()
    {
        CharacterController.enabled = false;
    }
}
