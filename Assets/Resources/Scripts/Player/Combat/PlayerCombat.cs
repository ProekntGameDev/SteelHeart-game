using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField, Required] private Player _player;
    [SerializeField, Required] private OverlapSphere _attackPoint;
    [SerializeField] private float _damage;
    [SerializeField] private float _delay;
    [SerializeField] private float _cooldown;
    [SerializeField] private float _staminaCost;

    private IdleCombatState _idleState;
    private AttackCombatState _attackState;

    private StateMachine _stateMachine;

    private void Start()
    {
        InitStateMachine();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_stateMachine == null)
            return;

        InitStateMachine();
    }
#endif

    private void InitStateMachine()
    {
        _stateMachine = new StateMachine();

        SetupStates();
        SetupTransitions();

        _stateMachine.SetState(_idleState);
    }

    private void SetupStates()
    {
        _idleState = new IdleCombatState();
        _attackState = new AttackCombatState(_attackPoint, _delay, _cooldown, _damage);
    }

    private void SetupTransitions()
    {
        _player.Input.Player.Fire.performed += (context) => OnFirePerformed(context);

        _stateMachine.AddTransition(_attackState, _idleState, () => _attackState.IsDone());
    }

    private void OnFirePerformed(InputAction.CallbackContext context)
    { 
        if(context.phase == InputActionPhase.Performed && _stateMachine.IsInState(_attackState) == false)
            _stateMachine.SetState(_attackState);
    }

    private void Update()
    {
        _stateMachine.Tick();
    }
}
