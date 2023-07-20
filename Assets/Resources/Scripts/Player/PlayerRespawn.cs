using UnityEngine;
using NaughtyAttributes;

[RequireComponent(typeof(Player))]
public class PlayerRespawn : MonoBehaviour
{
    [SerializeField, Required] private LevelData _levelParameters;

    private Player _player;

    private void Start()
    {
        _player = GetComponent<Player>();

        _levelParameters.respawnCheckpoint = transform.position;
        _player.Health.OnDeath.AddListener(Die);
    }

    private void Die()
    {
        _player.enabled = false;
    }

    public void Respawn()
    {
        _player.transform.position = _levelParameters.respawnCheckpoint;

        _player.Health.Heal(_player.Health.Max);

        _player.Movement.enabled = true;
    }
}
