using UnityEngine;
using NaughtyAttributes;

[RequireComponent(typeof(Health), typeof(PlayerRespawn))]
public class Player : MonoBehaviour
{
    public Health Health => _health;
    public Stamina Stamina => _stamina;
    public PlayerMovement Movement => _playerMovement;
    public PlayerCombat Combat => _playerCombat;
    public PlayerInput Input => _playerInput;
    public PlayerRespawn Respawn => _playerRespawn;
    public GearsHolder CoinHolder => _coinHolder;

    [SerializeField, Required] private Stamina _stamina;
    [SerializeField, Required] private PlayerMovement _playerMovement;
    [SerializeField, Required] private PlayerCombat _playerCombat;
    [SerializeField, Required] private PlayerInput _playerInput;
    [SerializeField, Required] private GearsHolder _coinHolder;

    private Health _health;
    private PlayerRespawn _playerRespawn;

    private void Awake()
    {
        _health = GetComponent<Health>();
        _playerRespawn = GetComponent<PlayerRespawn>();
    }

    private void OnEnable()
    {
        _health.enabled = true;
        _stamina.enabled = true;
        _playerMovement.enabled = true;
        _playerInput.enabled = true;
        _coinHolder.enabled = true;
        _playerCombat.enabled = true;
    }

    private void OnDisable()
    {
        _health.enabled = false;
        _stamina.enabled = false;
        _playerMovement.enabled = false;
        _playerInput.enabled = false;
        _coinHolder.enabled = false;
        _playerCombat.enabled = false;
    }
}
