using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class LevelEndTrigger : MonoBehaviour
{
    [HideInInspector] public UnityEvent OnLevelEnd;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            player.enabled = false;
            OnLevelEnd?.Invoke();
        }
    }
}
