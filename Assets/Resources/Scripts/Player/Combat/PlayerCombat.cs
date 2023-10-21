using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerCombat : MonoBehaviour
{
    private const float COMBO_ACTION_TIME = 2f;
    private const int COMBO_QUEUE_CAPACITY = 5;

    public bool IsInBlockState => _currentCombatState == _blockState;

    [SerializeField] private ParryState _parryState;
    [SerializeField] private StanState _stanState;
    [SerializeField] private BlockState _blockState;
    [SerializeField] private List<BaseCombatState> _combos;

    [Inject] private Player _player;

    private BaseCombatState _currentCombatState;

    private List<ComboAction> _inputComboList = new List<ComboAction>(COMBO_QUEUE_CAPACITY+1);

    public DamageBlockResult TryBlock(Damage damage, Health from)
    {
        DamageBlockResult result = DamageBlockResult.None;

        if (_currentCombatState != _blockState || from == null)
            return result;
        else
            result = _blockState.BlockDamage(damage);

        if (result == DamageBlockResult.Broken)
        {
            _currentCombatState?.OnInterrupt();
            _stanState.Init(_blockState.StanDuration);
            UpdateState(_stanState);
        }
        else if (result == DamageBlockResult.Reflected)
        {
            _parryState.ParryTarget = from;
            UpdateState(_parryState);
        }

        return result;
    }

    public void ResetCombot() => _inputComboList.Clear();

    private void UpdateState(BaseCombatState newState)
    {
        _currentCombatState?.Exit();
        _currentCombatState = newState;
        _currentCombatState?.Enter();
    }

    private void FixedUpdate()
    {
        _currentCombatState?.Tick();

        if (_currentCombatState != null)
        {
            if (_currentCombatState.IsDone())
                UpdateState(null);

            else if (_player.Movement.CanUseCombat == false)
            {
                if (_currentCombatState.IsInterruptible)
                    _currentCombatState.OnInterrupt();
                UpdateState(null);
            }
        }

        UpdateComboList();
    }

    private void AddActionToCombo(InputAction.CallbackContext context)
    {
        _inputComboList.Add(new ComboAction(context));

        while (_inputComboList.Count >= COMBO_QUEUE_CAPACITY)
            _inputComboList.RemoveAt(0);
    }

    private void UpdateComboList()
    {
        for (int i = 0; i < _inputComboList.Count; i++)
        {
            if (_inputComboList[i].IsExpired())
            {
                _inputComboList.RemoveAt(i);
                i -= 1;
            }
        }

        if (_currentCombatState != null || _player.Movement.CanUseCombat == false)
            return;

        foreach (var attack in _combos)
        {
            if (attack.IsInCombo(_inputComboList))
            {
                if (attack.IsReady())
                    UpdateState(attack);

                ResetCombot();
                return;
            }
        }
    }

    private void OnEnable()
    {
        _player.Input.Player.Block.performed += AddActionToCombo;
        _player.Input.Player.FireHold.performed += AddActionToCombo;
        _player.Input.Player.Fire.canceled += AddActionToCombo;
    }

    private void OnDisable()
    {
        _player.Input.Player.Block.performed -= AddActionToCombo;
        _player.Input.Player.FireHold.performed -= AddActionToCombo;
        _player.Input.Player.Fire.canceled -= AddActionToCombo;
    }

    public class ComboAction
    { 
        public float StartTime { get; private set; }
        public InputAction Action { get; private set; }

        public ComboAction(InputAction.CallbackContext context)
        {
            Action = context.action;
            StartTime = (float)context.startTime;
        }

        public bool IsExpired() => Time.time >= StartTime + COMBO_ACTION_TIME;
    }
}
