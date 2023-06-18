using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IPlayerBehaviourData, IControllable
{
    public CharacterController CharacterController => _characterController;
    public Transform PositionPlayer => _positionPlayer;
    public float X { get; private set; }
    public float Z { get; private set; }
    public float SpeedPlayer { get; private set; }
    public IPlayerAnimator PlayerAnimator { get; private set; }

    public IPlayerBehaviour PlayerBehaviour => _playerBehaviourController.CurrentPlayerBehaviour;

    [Header("PlayerBehaviourController")]
    [SerializeField] private PlayerBehaviourController _playerBehaviourController;

    [Header("PlayerData")]
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Transform _positionPlayer;
    [SerializeField] private Animator _playerAnimator;

    [Header("CheckIsGraunded")]
    [SerializeField] private CheckIsGrounded _checkIsGrounded;

    [Header("PlayerSpeed")]
    [SerializeField] private float _walkSpeed = 3f;
    [SerializeField] private float _runSpeed = 6f;

    private void Awake()
    {
        PlayerAnimator = GetComponent<IPlayerAnimator>();
    }

    private void Update()
    {
        if (PlayerBehaviour is JumpPlayerBehaviour && PlayerBehaviour.IsActive == false)
            _playerBehaviourController.SetFallPlayerBehaviour();
        else if (PlayerBehaviour is FallPlayerBehaviour && _checkIsGrounded != null && _checkIsGrounded.IsGrounded)
            _playerBehaviourController.SetIdlePlayerBehaviour();
        else if (PlayerBehaviour is IdlePlayerBehaviour || PlayerBehaviour is RunPlayerBehaviour || PlayerBehaviour is WalkPlayerBehaviour)
        {
            if (_checkIsGrounded != null && _checkIsGrounded.IsGrounded)
                _playerBehaviourController.SetFallPlayerBehaviour();
        }
    }

    public void Move(float x, float z, bool isShift)
    {
        X = x;
        Z = z;
        if (PlayerBehaviour is IdlePlayerBehaviour || PlayerBehaviour is RunPlayerBehaviour || PlayerBehaviour is WalkPlayerBehaviour)
        {
            if (_checkIsGrounded != null && _checkIsGrounded.IsGrounded)
            {
                if (X != 0 || Z != 0)
                {
                    if (isShift) _playerBehaviourController.SetRunPlayerBehaviour();
                    else _playerBehaviourController.SetWalkPlayerBehaviour();
                }
                else _playerBehaviourController.SetIdlePlayerBehaviour();
            }
        }
        if (isShift) SpeedPlayer = _runSpeed;
        else SpeedPlayer = _walkSpeed;
    }   

    public void Jump()
    {
        if (_checkIsGrounded != null && _checkIsGrounded.IsGrounded)
            _playerBehaviourController.SetJumpPlayerBehaviour();
    }
}
