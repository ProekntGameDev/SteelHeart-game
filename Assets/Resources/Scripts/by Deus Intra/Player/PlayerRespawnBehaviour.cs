using UnityEngine;

[RequireComponent(typeof(Health))]
public class PlayerRespawnBehaviour : MonoBehaviour
{
    public int amountOfCoinsForAdditionalLife = 100;

    private int _additionalLives = 1;
    public LevelParameters levelParameters;

    private Health _health;


    private void Awake()
    {
        _health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        _health.OnDeath += Die;
    }

    private void OnDisable()
    {
        _health.OnDeath -= Die;
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
        _health.FullHeal();
    }

    public void AddLifes(int amount)
    {
        _additionalLives += amount;
    }
}
