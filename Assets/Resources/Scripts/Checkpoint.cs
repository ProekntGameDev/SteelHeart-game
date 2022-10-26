using UnityEngine;
using Interfaces;

public class Checkpoint : MonoBehaviour, IInteractableMonoBehaviour
{
    public void Interact(Transform obj)
    {
        var _respawnBehaviour = obj.GetComponent<PlayerRespawn>();
        if (_respawnBehaviour == null) return;

        var levelParameters = _respawnBehaviour.levelParameters;
        levelParameters.respawnCheckpoint = transform.position;
    }
}
