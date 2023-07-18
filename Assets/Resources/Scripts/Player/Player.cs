using UnityEngine;
using NaughtyAttributes;

public class Player : MonoBehaviour
{
    public Health Health => _health;
    public PlayerMovement Movement => _playerMovement;
    public CoinHolder CoinHolder => _coinHolder;

    [SerializeField] private Health _health;
    [SerializeField, Required] private PlayerMovement _playerMovement;
    [SerializeField, Required] private CoinHolder _coinHolder;
}
