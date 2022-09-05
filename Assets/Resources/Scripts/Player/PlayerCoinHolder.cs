using UnityEngine;

public class PlayerCoinHolder : MonoBehaviour
{
    public delegate void VoidDelegate();
    public event VoidDelegate OnChange;

    public int Coins { get; private set; }
    private PlayerRespawnBehaviour _respawnBehaviour;


    private void Awake()
    {
        _respawnBehaviour = GetComponent<PlayerRespawnBehaviour>();
    }

    public void AddCoin(int worth)
    {
        Coins += worth;
        OnChange?.Invoke();

        if(_respawnBehaviour == null)
        {
            Debug.LogError("скрипт PlayerRespawnBehaviour не найден");
            return;
        }

        int lifeIncrement = _respawnBehaviour.amountOfCoinsForAdditionalLife;
        if (Coins > lifeIncrement)
        {
            int livesToAdd = Coins / lifeIncrement;
            _respawnBehaviour.AddLifes(livesToAdd);

            Coins %= lifeIncrement;
        }
    }
}
