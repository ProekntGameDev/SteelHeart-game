using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Gear : MonoBehaviour
{
    [SerializeField, Min(1)] private int _worth = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            player.GearsHolder.Increase(_worth);
            Destroy(gameObject);
        }
    }
}
