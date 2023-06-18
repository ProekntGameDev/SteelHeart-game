using UnityEngine;

public class FallPlayerBehaviour : MonoBehaviour, IPlayerBehaviour
{
    public bool IsActive { get; private set; }
    public IPlayerBehaviourData PlayerData { get; private set; }

    [SerializeField] private PlayerMove _playerMove;

    [SerializeField] private float _gravity;


    private void Awake()
    {
        PlayerData = GetComponent<IPlayerBehaviourData>();
    }

    public void EnterBehaviour()
    {
        IsActive = true;
    }
    public void UpdateBehaviour()
    {
        Fall();
    }
    public void ExitBehaviour()
    {
        IsActive = false;
    }

    private void Fall()
    {
        if (_playerMove != null)
            _playerMove.MovePlayer(PlayerData.Z, PlayerData.X, PlayerData.SpeedPlayer, PlayerData, _gravity);
    }
}
