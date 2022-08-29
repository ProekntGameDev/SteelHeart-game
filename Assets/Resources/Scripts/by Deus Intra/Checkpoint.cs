using UnityEngine;

public class Checkpoint : MonoBehaviour, IInteractableMonoBehaviour
{
    public void Interact(Transform obj)
    {
        var player = obj.GetComponent<PlayerRespawnBehaviour>();
        if (player == null) return;

        player.checkpoint = transform.position;
    }
}
