using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Zenject;
using NaughtyAttributes;

public class PlayerCombat : MonoBehaviour
{
    [HideInInspector] public UnityEvent OnAttack;

    [SerializeField, Required] private OverlapSphere _attackPoint;
    [SerializeField, Required] private OverlapSphere _punchAssistant;
    [SerializeField] private float _damage;
    [SerializeField] private float _delay;
    [SerializeField] private float _cooldown;
    [SerializeField] private float _staminaCost;

    [Inject] private Player _player;

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
        _attackState = new AttackCombatState(_player, _attackPoint, _punchAssistant, _delay, _cooldown, _damage);
    }

    private void SetupTransitions()
    {
        _player.Input.Player.Fire.performed += (context) => OnFirePerformed(context);

        _stateMachine.AddTransition(_attackState, _idleState, () => _attackState.IsDone());
    }

    private void OnFirePerformed(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed && _stateMachine.IsInState(_attackState) == false)
            return;

        if (_player.Movement.Ladder != null)
            return;

        _stateMachine.SetState(_attackState);
        OnAttack?.Invoke();
    }

    private void Update()
    {
        _stateMachine.Tick();
    }
}
