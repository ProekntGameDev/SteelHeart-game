using UnityEngine;

public class Player : MonoBehaviour
{
    public Health Health => _health;

    [SerializeField] private Health _health;
}
