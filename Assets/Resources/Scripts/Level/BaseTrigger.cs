using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public abstract class BaseTrigger : MonoBehaviour
{
    public UnityEvent OnInteract;

    [SerializeField] protected bool _oneUse;

    protected void Interact()
    {
        OnInteract?.Invoke();

        if (_oneUse)
            Destroy(this);
    }
}
