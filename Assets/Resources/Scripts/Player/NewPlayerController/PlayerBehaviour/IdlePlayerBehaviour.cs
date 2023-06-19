using UnityEngine;

public class IdlePlayerBehaviour : MonoBehaviour, IPlayerBehaviour
{
    public bool IsActive { get; private set; } //activity behavior
    public IPlayerBehaviourData PlayerData { get; private set; } //player data

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

    }
    public void ExitBehaviour()
    {
        IsActive = false;
    }
}
