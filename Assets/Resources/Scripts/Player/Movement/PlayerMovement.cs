using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Zenject;

namespace OldMovement
{
    [RequireComponent(typeof(InertialCharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [HideInInspector] public UnityEvent OnJump;
        [HideInInspector] public UnityEvent OnEnterLadder;
        [HideInInspector] public UnityEvent OnExitLadder;

        public bool IsInCrouch => _stateMachine.IsInState(_crouchState);
        public Ladder Ladder => _stateMachine.IsInState(_ladderMoveState) ? _ladderMoveState.Ladder : null;

        public InertialCharacterController CharacterController { get; private set; }

        [SerializeField] private float _idleStaminaRestoration;
        [SerializeField] private MoveState.Settings _walkSettings;
        [SerializeField] private MoveState.Settings _runSettings;
        [SerializeField] private MoveState.Settings _crouchSettings;
        [SerializeField] private MoveState.Settings _airSettings;
        [SerializeField] private float _crouchHeight = 1.6f;
        [SerializeField, Foldout("Ladder")] private float _ladderMoveSpeed = 2f;
        [SerializeField, Foldout("Ladder")] private OverlapSphere _ladderTrigger;
        [SerializeField] private JumpType _jumpType;
        [SerializeField] private float _jumpStaminaCost;

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

            if (_stateMachine.IsInState(_airMoveState) == false)
                return;

            LadderCheck();
        }

        private void InitStateMachine()
        {
            if (_stateMachine != null)
            {
                _player.Input.Player.Jump.performed -= OnPerformedJumpState;
                _player.Input.Player.Interact.performed -= OnInteractPerformed;

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
            _idleState = new IdleState(CharacterController, _player.Stamina, _walkSettings.MaxSpeed, _idleStaminaRestoration);
            _walkState = new MoveState(CharacterController, _player.Stamina, _walkSettings);
            _runState = new MoveState(CharacterController, _player.Stamina, _runSettings);
            _crouchState = new CrouchMoveState(CharacterController, _player.Stamina, _crouchHeight, _crouchSettings);
            _airMoveState = new AirMoveState(CharacterController, _player.Stamina, _airSettings);
            _jumpState = new JumpState(CharacterController, _player.Stamina, _airSettings, _jumpType, _jumpStaminaCost);
            _ladderMoveState = new LadderMoveState(CharacterController, _player.Stamina, _ladderTrigger.transform, _ladderMoveSpeed);
        }

        private void SetupTransitions()
        {
            // Base movement

            _stateMachine.AddTransition(_idleState, _walkState, () => CharacterController.ReadInputAxis().sqrMagnitude != 0);
            _stateMachine.AddTransition(_walkState, _idleState, () => CharacterController.ReadInputAxis().sqrMagnitude == 0);

            _stateMachine.AddTransition(_idleState, _crouchState, () => _player.Input.Player.Crouch.ReadValue<float>() > 0);
            _stateMachine.AddTransition(_walkState, _crouchState, () => _player.Input.Player.Crouch.ReadValue<float>() > 0);
            _stateMachine.AddTransition(_crouchState, _walkState, () => _player.Input.Player.Crouch.ReadValue<float>() <= 0 && CanStandUp());

            _stateMachine.AddTransition(_walkState, _runState, () => _player.Input.Player.Run.ReadValue<float>() > 0 && _player.Stamina.Current > _runState.StaminaCost);
            _stateMachine.AddTransition(_runState, _walkState, () => _player.Input.Player.Run.ReadValue<float>() <= 0 || _player.Stamina.Current < _runState.StaminaCost);

            // Jump and air movement

            _stateMachine.AddAnyTransition(_airMoveState, () => CanEnterAirState());

            _player.Input.Player.Jump.performed += OnPerformedJumpState;

            _stateMachine.AddTransition(_jumpState, _idleState, () => CharacterController.IsGrounded && _jumpState.StaminaCost <= _player.Stamina.Current);

            _stateMachine.AddTransition(_airMoveState, _idleState, () => CharacterController.IsGrounded);
            _stateMachine.AddTransition(_idleState, _airMoveState, () => CharacterController.IsGrounded == false);

            // Ladder

            _player.Input.Player.Interact.performed += OnInteractPerformed;

            _stateMachine.AddTransition(_ladderMoveState, _idleState, () => _ladderMoveState.IsOnLadder() == false && CharacterController.IsGrounded);
            _stateMachine.AddTransition(_ladderMoveState, _airMoveState, () => _ladderMoveState.IsOnLadder() == false && CharacterController.IsGrounded == false);
        }

        private bool CanEnterAirState()
        {
            return
                CharacterController.IsGrounded == false &&
                _jumpState.IsDone() &&
                _stateMachine.IsInState(_ladderMoveState) == false &&
                TryStepOnSlope() == false;
        }

        private void OnInteractPerformed(InputAction.CallbackContext context) => LadderCheck();

        private void LadderCheck()
        {
            foreach (var collider in _ladderTrigger.GetColliders())
                if (collider.TryGetComponent(out Ladder ladder))
                    InteractWithLadder(ladder);
        }

        private void InteractWithLadder(Ladder ladder)
        {
            if (_stateMachine.IsInState(_ladderMoveState))
                return;

            if (Vector3.Dot(CharacterController.CurrentVelocity.normalized, -ladder.transform.forward) < 0.5f)
                return;

            Vector3 ladderDirection = transform.position - ladder.transform.position;
            ladderDirection.Normalize();

            float angle = Vector3.Angle(ladderDirection, ladder.transform.forward);
            if (angle > 90)
                return;

            _ladderMoveState.SetLadder(ladder);

            if (_ladderMoveState.IsClimbOnLadder())
            {
                _ladderMoveState.ResetLadder();
                return;
            }

            _stateMachine.SetState(_ladderMoveState);
            OnEnterLadder?.Invoke();
        }

        private void OnPerformedJumpState(InputAction.CallbackContext context)
        {
            if (enabled == false || _stateMachine.IsInState(_crouchState) == true)
                return;

            if (_stateMachine.IsInState(_ladderMoveState))
            {
                _stateMachine.SetState(_idleState);
                OnExitLadder?.Invoke();
                return;
            }

            if (CharacterController.IsGrounded && _stateMachine.IsInState(_jumpState) == false)
            {
                _stateMachine.SetState(_jumpState);
                OnJump?.Invoke();
            }
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

        private bool CanStandUp() => CharacterController.CanSetHeight(_crouchState.StandHeight);

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
}
