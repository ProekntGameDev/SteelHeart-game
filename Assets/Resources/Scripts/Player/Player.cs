using UnityEngine;
using NaughtyAttributes;

[RequireComponent(typeof(Health), typeof(PlayerRespawn))]
public class Player : MonoBehaviour
{
    public PlayerInput Input { 
        get {
            if (_playerInput == null)
                _playerInput = new PlayerInput();

            return _playerInput;
        } 
    }

    public Journal Journal => _journal;
    public Health Health => _health;
    public Stamina Stamina => _stamina;
    public PlayerMovement Movement => _playerMovement;
    public PlayerCombat Combat => _playerCombat;
    public PlayerRespawn Respawn => _playerRespawn;
    public GearsHolder GearsHolder => _gearsHolder;

    [SerializeField, Required] private Journal _journal;
    [SerializeField, Required] private Stamina _stamina;
    [SerializeField, Required] private PlayerMovement _playerMovement;
    [SerializeField, Required] private PlayerCombat _playerCombat;
    [SerializeField, Required] private GearsHolder _gearsHolder;

    private PlayerInput _playerInput;
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
        _gearsHolder.enabled = true;
        _playerCombat.enabled = true;

        Input.Enable();
    }

    private void OnDisable()
    {
        _health.enabled = false;
        _stamina.enabled = false;
        _playerMovement.enabled = false;
        _gearsHolder.enabled = false;
        _playerCombat.enabled = false;

        Input.Disable();
    }
}
