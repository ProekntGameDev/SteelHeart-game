using UnityEngine;

public class WalkPlayerBehaviour : MonoBehaviour, IPlayerBehaviour
{
    public bool IsActive { get; private set; }
    public IPlayerBehaviourData PlayerData { get; private set; }

    [SerializeField] private PlayerMove _playerMove;

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
        Walk();
    }
    public void ExitBehaviour()
    {
        IsActive = false;
    }

    private void Walk()
    {
        if (_playerMove != null)
            _playerMove.MovePlayer(PlayerData.Z, PlayerData.X, PlayerData.SpeedPlayer, PlayerData, 0);
    }
}
