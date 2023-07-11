using NaughtyAttributes;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField, Required] private AttackPoint _attackPoint;
    [SerializeField] private float _damage;
    [SerializeField] private float _speed;
    [SerializeField] private float _staminaCost;

    private IdleCombatState _idleState;
    private AttackCombatState _attackState;

    private StateMachine _stateMachine;

    private void Awake()
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
        _attackState = new AttackCombatState(_attackPoint, _speed, _damage);
    }

    private void SetupTransitions()
    {
        _stateMachine.AddTransition(_idleState, _attackState, () => Input.GetMouseButtonDown(0));
        _stateMachine.AddTransition(_attackState, _idleState, () => _attackState.IsDone());
    }

    private void Update()
    {
        _stateMachine.Tick();
    }
}
