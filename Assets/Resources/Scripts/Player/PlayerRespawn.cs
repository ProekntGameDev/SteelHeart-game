using UnityEngine;

[RequireComponent(typeof(Health))]
public class PlayerRespawn : MonoBehaviour
{
    public int amountOfCoinsForAdditionalLife = 100;

    private int _additionalLives = 1;
    public LevelData levelParameters;

    private Health _health;


    private void Awake()
    {
        _health = GetComponent<Health>();
    }

    private void Start()
    {
        levelParameters.respawnCheckpoint = transform.position;
        _health.OnDeath.AddListener(Die);
    }

    public void Die()
    {
        if (_additionalLives > 0)
        {
            _additionalLives -= 1;
            Respawn();
        }
        else
        {
            Debug.Log("Death!");
            foreach (var component in GetComponents<MonoBehaviour>())
            {
                component.enabled = false;
            }
        }
    }

    public void Respawn()
    {
        Debug.Log("Respawn!");
        //player's transform is _health.transform;
        _health.transform.position = levelParameters.respawnCheckpoint;
        _health.Heal(_health.Max);
    }

    public void AddLifes(int amount)
    {
        _additionalLives += amount;
    }
}
