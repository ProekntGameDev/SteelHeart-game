using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

[RequireComponent(typeof(InertialCharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [HideInInspector] public UnityEvent OnJump;
    [HideInInspector] public UnityEvent OnEnterLadder;
    [HideInInspector] public UnityEvent OnExitLadder;

    public InertialCharacterController CharacterController { get; private set; }

    [SerializeField, Foldout("Max Velocities")] private float _maxCrouchVelocity = 4;
    [SerializeField, Foldout("Max Velocities")] private float _maxWalkVelocity = 6;
    [SerializeField, Foldout("Max Velocities")] private float _maxRunVelocity = 10;
    [SerializeField, Foldout("Max Velocities")] private float _maxAirVelocity = 100;
    [SerializeField, Foldout("Accelerations")] private float _groundAcceleration = 150;
    [SerializeField, Foldout("Accelerations")] private float _airAcceleration = 1500;
    [SerializeField] private float _crouchHeight = 1.6f;
    [SerializeField] private float _ladderMoveSpeed = 2f;
    [SerializeField] private OverlapSphere _ladderTrigger;
    [SerializeField] private JumpType _jumpType;

    [Inject] private Player _player;

    private AnimatableStateMachine _stateMachine;

    private IdleState _idleState;
    private MoveState _walkState;
    private MoveState _runState;
    private CrouchMoveState _crouchState;
    private AirMoveState _airMoveState;
    private JumpState _jumpState;
    private LadderMoveState _ladderMoveState;

    private void Awake()
    {
        CharacterController = GetComponent<InertialCharacterController>();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_stateMachine == null)
            return;

        InitStateMachine();
    }
#endif

    private void FixedUpdate()
    {
        CharacterController.ApplyGravity();

        _stateMachine.Tick();
    }

    private void InitStateMachine()
    {
        if (_stateMachine != null)
        {
            _player.Input.Player.Jump.performed -= (context) => OnPerformedJumpState();
            _player.Input.Player.Interact.performed -= (context) => OnInteractPerformed();

            _stateMachine.Clear();
        }
        else
            _stateMachine = new AnimatableStateMachine();

        SetupStates();
        SetupTransitions();

        _stateMachine.SetState(_idleState);
    }

    private void SetupStates()
    {
        _idleState = new IdleState(CharacterController, _maxWalkVelocity);
        _walkState = new MoveState(CharacterController, _groundAcceleration, _maxWalkVelocity);
        _runState = new MoveState(CharacterController, _groundAcceleration, _maxRunVelocity);
        _crouchState = new CrouchMoveState(CharacterController, _crouchHeight, _groundAcceleration, _maxCrouchVelocity);
        _airMoveState = new AirMoveState(CharacterController, _airAcceleration, _maxAirVelocity);
        _jumpState = new JumpState(CharacterController, _airAcceleration, _maxAirVelocity, _jumpType);
        _ladderMoveState = new LadderMoveState(CharacterController, _ladderTrigger.transform, _ladderMoveSpeed);
    }

    private void SetupTransitions()
    {
        // Base movement

        _stateMachine.AddTransition(_idleState, _walkState, () => CharacterController.ReadInputAxis().sqrMagnitude != 0);
        _stateMachine.AddTransition(_walkState, _idleState, () => CharacterController.ReadInputAxis().sqrMagnitude == 0);

        _stateMachine.AddTransition(_walkState, _crouchState, () => _player.Input.Player.Crouch.ReadValue<float>() > 0);
        _stateMachine.AddTransition(_crouchState, _walkState, () => _player.Input.Player.Crouch.ReadValue<float>() <= 0);

        _stateMachine.AddTransition(_walkState, _runState, () => _player.Input.Player.Run.ReadValue<float>() > 0);
        _stateMachine.AddTransition(_runState, _walkState, () => _player.Input.Player.Run.ReadValue<float>() <= 0);

        // Jump and air movement

        _stateMachine.AddAnyTransition(_airMoveState, () => CharacterController.IsGrounded == false && _jumpState.IsDone() && _stateMachine.IsInState(_ladderMoveState) == false);

        _player.Input.Player.Jump.performed += (context) => OnPerformedJumpState();

        _stateMachine.AddTransition(_jumpState, _idleState, () => CharacterController.IsGrounded);

        _stateMachine.AddTransition(_airMoveState, _idleState, () => CharacterController.IsGrounded);
        _stateMachine.AddTransition(_idleState, _airMoveState, () => CharacterController.IsGrounded == false);

        // Ladder

        _player.Input.Player.Interact.performed += (context) => OnInteractPerformed();

        _stateMachine.AddTransition(_ladderMoveState, _idleState, () => _ladderMoveState.IsOnLadder() == false);
    }

    private void OnInteractPerformed()
    {
        foreach (var collider in _ladderTrigger.GetColliders())
            if (collider.TryGetComponent(out Ladder ladder))
                InteractWithLadder(ladder);
    }

    private void InteractWithLadder(Ladder ladder)
    {
        if (_stateMachine.IsInState(_ladderMoveState))
        {
            _stateMachine.SetState(_idleState);
            OnExitLadder?.Invoke();
            return;
        }

        _ladderMoveState.Init(ladder);
        _stateMachine.SetState(_ladderMoveState);
        OnEnterLadder?.Invoke();
    }

    private void OnPerformedJumpState()
    {
        if (_stateMachine.IsInState(_ladderMoveState))
        {
            _stateMachine.SetState(_idleState);
            OnExitLadder?.Invoke();
            return;
        }

        if ((CharacterController.IsGrounded) && _stateMachine.IsInState(_jumpState) == false)
        {
            _stateMachine.SetState(_jumpState);
            OnJump?.Invoke();
        }
    }

    private void OnEnable()
    {
        InitStateMachine();

        CharacterController.enabled = true;
    }

    private void OnDisable()
    {
        CharacterController.CurrentVelocity = Vector3.zero;
        CharacterController.VerticalVelocity = 0;

        CharacterController.enabled = false;
    }
}
