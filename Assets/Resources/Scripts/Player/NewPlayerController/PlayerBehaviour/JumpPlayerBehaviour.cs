using UnityEngine;

public class JumpPlayerBehaviour : MonoBehaviour, IPlayerBehaviour
{
    public bool IsActive { get; private set; }
    public IPlayerBehaviourData PlayerData { get; private set; }

    [SerializeField] private PlayerMove _playerMove;

    [SerializeField] private float _jumpForce;
    
    private const float Gravity = -9.81f;//The acceleration with which it slows down

    private float _jumpSpeed;//Speed with he fly

    private void Awake()
    {
        PlayerData = GetComponent<IPlayerBehaviourData>();
    }

    public void EnterBehaviour()
    {
        IsActive = true;
        _jumpSpeed = _jumpForce;
    }
    public void UpdateBehaviour()
    {
        Jump();
    }
    public void ExitBehaviour()
    {
        IsActive = false;
    }

    private void Jump()
    {
        _jumpSpeed += Gravity * Time.deltaTime * 3f;

        if(_playerMove != null)
            _playerMove.JumpPlayer(PlayerData.Z, PlayerData.X, PlayerData.SpeedPlayer, PlayerData, _jumpSpeed);

        if(_jumpSpeed <= 0) IsActive = false;
    }
}
