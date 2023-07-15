using UnityEngine;
using NaughtyAttributes;

public class Player : MonoBehaviour
{
    public Health Health => _health;
    public InertialCharacterController CharacterController => _characterController;

    [SerializeField] private Health _health;
    [SerializeField] private InertialCharacterController _characterController;
}
