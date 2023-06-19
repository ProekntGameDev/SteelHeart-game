using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IPlayerBehaviourData, IControllable
{
    //data for behaviors
    public CharacterController CharacterController => _characterController;
    public Transform TransformPlayer => _transformPlayer;
    public float X { get; private set; }
    public float Z { get; private set; }
    public float SpeedPlayer { get; private set; }
    public IPlayerAnimator PlayerAnimator { get; private set; }

    public IPlayerBehaviour PlayerBehaviour => _playerBehaviourController.CurrentPlayerBehaviour; //current player behavior

    [Header("PlayerBehaviourController")]
    [SerializeField] private PlayerBehaviourController _playerBehaviourController; //managing player behaviors

    [Header("PlayerData")]
    [SerializeField] private CharacterController _characterController; //player CharacterController
    [SerializeField] private Transform _transformPlayer; //player Transform

    [Header("CheckIsGraunded")]
    [SerializeField] private CheckIsGrounded _checkIsGrounded; //check is grounded

    [Header("PlayerSpeed")]
    [SerializeField] private float _walkSpeed = 3f; //walking speed
    [SerializeField] private float _runSpeed = 6f; //running speed

    private void Awake()
    {
        PlayerAnimator = GetComponent<IPlayerAnimator>(); //receiving IPlayerAnimator
    }

    private void Update()
    {
        if (PlayerBehaviour is JumpPlayerBehaviour && !PlayerBehaviour.IsActive) //if the jump is over
            _playerBehaviourController.SetFallPlayerBehaviour(); //transition to behavior Fall
        else if (PlayerBehaviour is FallPlayerBehaviour && _checkIsGrounded != null && _checkIsGrounded.IsGrounded) //if the player landed
            _playerBehaviourController.SetIdlePlayerBehaviour(); //transition to behavior Idle
        else if (PlayerBehaviour is IdlePlayerBehaviour || PlayerBehaviour is RunPlayerBehaviour || PlayerBehaviour is WalkPlayerBehaviour) //if the player does not jump
            if (_checkIsGrounded != null && !_checkIsGrounded.IsGrounded) _playerBehaviourController.SetFallPlayerBehaviour(); //if not on earth then transition to behavior Fall
    }

    public void Move(float x, float z, bool isShift)
    {
        X = x;
        Z = z;
        if (PlayerBehaviour is IdlePlayerBehaviour || PlayerBehaviour is RunPlayerBehaviour || PlayerBehaviour is WalkPlayerBehaviour) //if the player does not jump and does not fall
        {
            if (_checkIsGrounded != null && _checkIsGrounded.IsGrounded) //if the player is standing on the ground
            {
                if (X != 0 || Z != 0) //if the player moves
                {
                    if (isShift) _playerBehaviourController.SetRunPlayerBehaviour(); //transition to behavior Run
                    else _playerBehaviourController.SetWalkPlayerBehaviour(); //transition to behavior Walk
                }
                else _playerBehaviourController.SetIdlePlayerBehaviour(); //transition to behavior Idle
            }
        }
        if (isShift) SpeedPlayer = _runSpeed;
        else SpeedPlayer = _walkSpeed;
    }   

    public void Jump()
    {
        if (_checkIsGrounded != null && _checkIsGrounded.IsGrounded) //if the player is standing on the ground
            _playerBehaviourController.SetJumpPlayerBehaviour(); //transition to behavior Jump
    }
}
