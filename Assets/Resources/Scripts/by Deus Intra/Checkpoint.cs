using UnityEngine;

public class Checkpoint : MonoBehaviour, IInteractableMonoBehaviour
{
    public void Interact(Transform obj)
    {
        var _respawnBehaviour = obj.GetComponent<PlayerRespawnBehaviour>();
        if (_respawnBehaviour == null) return;

        var levelParameters = _respawnBehaviour.levelParameters;
        levelParameters.respawnCheckpoint = transform.position;
    }
}
