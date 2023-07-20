using UnityEngine;
using UnityEngine.Events;

public abstract class ItemReceiver : MonoBehaviour
{
    public bool IsReceived { get; private set; } = false;

    public UnityEvent OnReceived;

    public bool TryReceive(Pickupable item)
    {
        if (Check(item) == false)
            return false;

        item.Deliver();

        IsReceived = true;
        OnReceived?.Invoke();

        return true;
    }

    protected abstract bool Check(Pickupable item);
}
