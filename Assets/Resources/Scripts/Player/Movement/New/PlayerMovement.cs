using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Zenject;

[RequireComponent(typeof(InertialCharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [HideInInspector] public UnityEvent OnJump;
    [HideInInspector] public UnityEvent<Ladder> OnLadderInteract;

    public bool CanUseCombat => _currentMovementState.CanUseCombat();
    public bool IsOnLadder => _currentMovementState == _ladderMovement;

    public InertialCharacterController CharacterController 
    {
        get
        {
            if (_characterController == null)
                _characterController = GetComponent<InertialCharacterController>();
            return _characterController;
        }
    }

    [SerializeField, Required] private StandardMovement _standardMovement;
    [SerializeField, Required] private RollMovement _rollMovement;
    [SerializeField, Required] private LadderMovement _ladderMovement;
    [SerializeField, Required] private OverlapSphere _ladderTrigger;

    [Inject] private Player _player;

    private BaseMovementState _currentMovementState;
    private InertialCharacterController _characterController;

    private void Awake()
    {
        UpdateState(_standardMovement);
    }

    private void FixedUpdate()
    {
        CharacterController.ApplyGravity();

        _currentMovementState?.Tick();

        if (IsOnLadder)
        {
            if (_ladderMovement.IsOnLadder() == false)
                UpdateState(_standardMovement);
        }
        else if (CharacterController.IsGrounded == false)
            LadderCheck();

        if (_currentMovementState == _rollMovement && _rollMovement.IsDone())
            UpdateState(_standardMovement);
            
    }

    private void UpdateState(BaseMovementState newMovementState)
    {
        if (_currentMovementState == newMovementState)
            throw new System.ArgumentException(nameof(newMovementState));

        _currentMovementState?.Exit();
        _currentMovementState = newMovementState;
        _currentMovementState?.Enter();
    }

    private void JumpPerformed(InputAction.CallbackContext context)
    {
        if (_currentMovementState != _ladderMovement)
            _currentMovementState?.OnJump();
        else
            UpdateState(_standardMovement);
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
        if (_currentMovementState == _ladderMovement)
            return;

        if (Vector3.Dot(CharacterController.CurrentVelocity.normalized, -ladder.transform.forward) < 0.5f)
            return;

        Vector3 ladderDirection = transform.position - ladder.transform.position;
        ladderDirection.Normalize();

        float angle = Vector3.Angle(ladderDirection, ladder.transform.forward);
        if (angle > 90)
            return;

        _ladderMovement.Init(ladder);
        UpdateState(_ladderMovement);
        OnLadderInteract?.Invoke(ladder);
    }

    private void OnRollPerformed(InputAction.CallbackContext context)
    {
        if (_currentMovementState == _ladderMovement || _currentMovementState == _rollMovement)
            return;

        if (_player.Stamina.Current < _rollMovement.StaminaCost || CharacterController.IsGrounded == false)
            return;

        UpdateState(_rollMovement);
    }

    private void OnEnable()
    {
        _player.Input.Player.Interact.performed += OnInteractPerformed;
        _player.Input.Player.Roll.performed += OnRollPerformed;
        _player.Input.Player.Jump.performed += JumpPerformed;
    }

    private void OnDisable()
    {
        _player.Input.Player.Interact.performed -= OnInteractPerformed;
        _player.Input.Player.Roll.performed -= OnRollPerformed;
        _player.Input.Player.Jump.performed -= JumpPerformed;
    }
}
