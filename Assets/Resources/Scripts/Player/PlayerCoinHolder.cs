using UnityEngine;
using UnityEngine.Events;

public class PlayerCoinHolder : MonoBehaviour
{
    [HideInInspector] public UnityEvent<int> OnChange;

    public int Coins { get; private set; }

    private PlayerRespawn _respawnBehaviour;

    private void Awake()
    {
        _respawnBehaviour = GetComponent<PlayerRespawn>();
    }

    public void Increase(int value)
    {
        Coins += value;

        TryAddAdditionalLifes();

        OnChange?.Invoke(Coins);
    }

    private void TryAddAdditionalLifes()
    {
        int lifeIncrement = _respawnBehaviour.amountOfCoinsForAdditionalLife;
        if (Coins > lifeIncrement)
        {
            int livesToAdd = Coins / lifeIncrement;
            _respawnBehaviour.AddLifes(livesToAdd);

            Coins %= lifeIncrement;
        }
    }
}
