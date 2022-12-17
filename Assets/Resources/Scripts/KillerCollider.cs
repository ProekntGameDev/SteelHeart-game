using UnityEngine;

public class KillerCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Player>(out var player)) return;

        player.Health.Kill();
    }
}