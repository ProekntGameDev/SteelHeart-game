using UnityEngine;
using NaughtyAttributes;

[RequireComponent(typeof(Player))]
public class PlayerRespawn : MonoBehaviour
{
    [SerializeField, Required] private LevelData _levelParameters;

    private Player _player;
    private int _extraLives;

    private void Start()
    {
        _player = GetComponent<Player>();

        _levelParameters.respawnCheckpoint = transform.position;
        _player.Health.OnDeath.AddListener(Die);
    }

    private void Die()
    {
        _player.Movement.enabled = false;

        // Temp logic. UI for that doesn't exist for now
        if (_extraLives > 0)
        {
            _extraLives -= 1;
            Respawn();
            return;
        }

        foreach (var component in _player.GetComponents<MonoBehaviour>())
            component.enabled = false;
    }

    public void Respawn()
    {
        Debug.Log("Respawn!");

        _player.transform.position = _levelParameters.respawnCheckpoint;

        _player.Health.Heal(_player.Health.Max);

        _player.Movement.enabled = true;
    }

    public void AddLifes(int amount)
    {
        _extraLives += amount;
    }
}
