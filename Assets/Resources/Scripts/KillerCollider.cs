using UnityEngine;

public class KillerCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
            player.Health.TakeDamage(player.Health.Max);
    }
}