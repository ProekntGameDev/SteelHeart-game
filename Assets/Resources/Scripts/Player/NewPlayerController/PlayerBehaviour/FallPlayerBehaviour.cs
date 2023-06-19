using UnityEngine;

public class FallPlayerBehaviour : MonoBehaviour, IPlayerBehaviour
{
    public bool IsActive { get; private set; } //activity behavior
    public IPlayerBehaviourData PlayerData { get; private set; } //player data

    [SerializeField] private PlayerMove _playerMove; //player movement

    [SerializeField] private float _gravity; //gravity


    private void Awake()
    {
        PlayerData = GetComponent<IPlayerBehaviourData>(); //receiving IPlayerBehaviourData
    }

    public void EnterBehaviour()
    {
        IsActive = true;
    }
    public void UpdateBehaviour()
    {
        Fall(); //player fall
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
