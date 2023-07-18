using UnityEngine;
using UnityEngine.Events;

public class CoinHolder : MonoBehaviour
{
    [HideInInspector] public UnityEvent<int> OnChange;

    public int Coins { get; private set; }

    public void Increase(int value)
    {
        if (value < 0)
            throw new System.ArgumentOutOfRangeException(nameof(value));

        Coins += value;

        OnChange?.Invoke(Coins);
    }

    public void Reduce(int value)
    {
        if (value > 0)
            throw new System.ArgumentOutOfRangeException(nameof(value));

        Coins -= value;

        OnChange?.Invoke(Coins);
    }
}
