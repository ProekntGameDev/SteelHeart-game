using UnityEngine;
using UnityEngine.Events;

public class ItemReceiver : MonoBehaviour
{
    public bool IsReceived { get; private set; } = false;

    public UnityEvent OnReceived;

    public void ReceiveItem()
    {
        IsReceived = true;
        OnReceived?.Invoke();
    }
}
