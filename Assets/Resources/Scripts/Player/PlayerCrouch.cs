using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerCrouch : MonoBehaviour
{
    public KeyCode crouchButton = KeyCode.S;

    public bool IsCrouchButtonPressed { get; private set; }

    private PlayerMovement _movementController;
    private CapsuleCollider _collider;
    private bool _isCrouching = false;
    private bool _isOnFloor = false;


    private void Awake()
    {
        _collider = GetComponent<CapsuleCollider>();
        _movementController = GetComponent<PlayerMovement>();        
    }

    private void Update()
    {
        IsCrouchButtonPressed = Input.GetKey(crouchButton);
        //_isOnFloor = _movementController.IsOnFloor;
    }

    private void FixedUpdate()
    {
        if (IsCrouchButtonPressed && _isOnFloor && _isCrouching == false) 
        { 
            _collider.height /= 2; 
            _isCrouching = true; 
        }
        else if (_isCrouching && IsCrouchButtonPressed == false) 
        { 
            _collider.height *= 2; 
            _isCrouching = false; 
        }
    }    
}
