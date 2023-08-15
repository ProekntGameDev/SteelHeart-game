using UnityEngine;
using UnityEngine.Events;

public class Trigger : BaseTrigger
{
    public UnityEvent OnExit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player) == false)
            return;

        Interact();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player) == false)
            return;

        OnExit?.Invoke();
    }
}
