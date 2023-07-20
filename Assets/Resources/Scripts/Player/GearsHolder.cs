using UnityEngine;
using UnityEngine.Events;

public class GearsHolder : MonoBehaviour
{
    [HideInInspector] public UnityEvent<int> OnChange;

    public int Gears { get; private set; }

    public void Increase(int value)
    {
        if (value < 0)
            throw new System.ArgumentOutOfRangeException(nameof(value));

        Gears += value;

        OnChange?.Invoke(Gears);
    }

    public void Reduce(int value)
    {
        if (value > 0)
            throw new System.ArgumentOutOfRangeException(nameof(value));

        Gears -= value;

        OnChange?.Invoke(Gears);
    }
}
