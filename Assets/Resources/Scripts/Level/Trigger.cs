using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Trigger : MonoBehaviour
{
    public UnityEvent OnInteract;

    [SerializeField] protected bool _oneUse;

    protected virtual void OnTriggerEnter(Collider other)
    {
        Interact();
    }

    protected void Interact()
    {
        OnInteract?.Invoke();

        if (_oneUse)
            Destroy(this);
    }
}
