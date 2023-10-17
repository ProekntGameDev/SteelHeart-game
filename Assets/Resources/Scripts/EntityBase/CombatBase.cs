using NaughtyAttributes;
using UnityEngine;
using Zenject;

public abstract class CombatBase : MonoBehaviour
{
    [SerializeField, Required] private Health _health; 
    [SerializeField, Required] private Stamina _stamina;

    [Inject] private Player _player;

    private StateMachine _stateMachine;
}
