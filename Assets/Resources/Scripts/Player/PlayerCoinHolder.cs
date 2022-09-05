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

        if (_respawnBehaviour != null)
        {
            int lifeIncrement = _respawnBehaviour.amountOfCoinsForAdditionalLife;
            if (Coins > lifeIncrement)
            {
                int livesToAdd = Coins / lifeIncrement;
                _respawnBehaviour.AddLifes(livesToAdd);

                Coins %= lifeIncrement;
            }
        }
        else
        {
            Debug.LogError("скрипт PlayerRespawnBehaviour не найден");
        }
        OnChange?.Invoke();
    }
}
