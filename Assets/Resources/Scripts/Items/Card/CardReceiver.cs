using UnityEngine;
using UnityEngine.Events;

public class CardReceiver : ItemReceiver
{
    public UnityEvent OnSuccess;
    public UnityEvent OnFailure;

    [SerializeField, Min(0)] private int _minCardLevel;

    protected override bool Check(Pickupable item)
    {
        if (item is Card == false)
        {
            OnFailure?.Invoke();
            return false;
        }

        bool result = ((Card)item).Level >= _minCardLevel;

        if (result)
            OnSuccess?.Invoke();
        else
            OnFailure?.Invoke();

        return result;
    }
}
