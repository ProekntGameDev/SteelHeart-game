using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public abstract class BaseCombatState : MonoBehaviour
{
    public abstract bool IsInterruptible { get; }

    public abstract void Enter();

    protected abstract InputAction[] _buttons { get; }

    public virtual bool CanEnter() => true;
    public virtual bool IsDone() => true;
    public virtual bool IsReady() => _endTime + _cooldown <= Time.time && _player.Stamina.Current >= _staminaCost;

    public virtual void Tick()
    {
        RotateToClosestEnemy();
    }

    public virtual void Exit()
    {
        _endTime = Time.time;

        _player.Stamina.Decay(_staminaCost);
    }

    public virtual void OnInterrupt()
    { }

    [SerializeField, Required] protected OverlapSphere _punchAssistant;
    [SerializeField] protected float _staminaCost;
    [SerializeField] protected float _cooldown;

    [Inject] protected Player _player;

    protected float _endTime;

    public bool IsInCombo(IReadOnlyList<PlayerCombat.ComboAction> comboActions)
    {
        if (_buttons == null || comboActions == null)
            return false;

        for (var i = comboActions.Count - 1; i >= _buttons.Length - 1; i--)
        {
            bool found = true;

            for (var j = _buttons.Length - 1; j >= 0 && found; j--)
                found = comboActions[i - (_buttons.Length - 1 - j)].Action == _buttons[j];

            if (found)
                return true;
        }

        return false;
    }

    protected void RotateToClosestEnemy()
    {
        Collider targetCollider = _punchAssistant.GetColliders()
            .Where(x => x.TryGetComponent(out IDamagable damagable) && damagable != (IDamagable)_player.Health)
            .OrderBy(x => Vector3.Distance(x.transform.position, _player.transform.position))
            .FirstOrDefault();

        if (targetCollider == null)
            return;

        RotateToTarget(targetCollider.transform.position);
    }

    protected void RotateToTarget(Vector3 target)
    {
        Vector3 direction = target - _player.transform.position;
        direction.y = 0;
        direction.Normalize();

        _player.Movement.CharacterController.Rotate(direction, 1);
    }
}
