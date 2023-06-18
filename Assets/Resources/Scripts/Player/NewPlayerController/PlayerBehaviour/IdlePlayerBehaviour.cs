using UnityEngine;

public class IdlePlayerBehaviour : MonoBehaviour, IPlayerBehaviour
{
    public bool IsActive { get; private set; }
    public IPlayerBehaviourData PlayerData { get; private set; }

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

    }
    public void ExitBehaviour()
    {
        IsActive = false;
    }
}
