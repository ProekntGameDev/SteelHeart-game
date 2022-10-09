using UnityEngine;
using Interfaces;

public class Thorns : MonoBehaviour, ITriggerableMonoBehaviour
{
    public void Trigger(Transform obj)
    {
        var respawnBehaviour = obj.GetComponent<PlayerRespawn>();
        if (respawnBehaviour != null)
        {
            respawnBehaviour.Die();
        }
    }
}
