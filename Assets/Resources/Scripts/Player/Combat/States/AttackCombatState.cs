using System.Collections;
using System.Linq;
using UnityEngine;

public class AttackCombatState : IState
{
    private Player _player;
    private OverlapSphere _attackPoint;
    private OverlapSphere _punchAssistant;
    private float _damage;
    private float _delay;
    private float _cooldown;

    private IEnumerator _attackCoroutine;

    public AttackCombatState(Player player, OverlapSphere attackPoint, OverlapSphere punchAssistant, float delay, float cooldown, float damage)
    {
        _player = player;
        _attackPoint = attackPoint;
        _punchAssistant = punchAssistant;
        _delay = delay;
        _cooldown = cooldown;
        _damage = damage;
    }

    public void OnEnter()
    {
        _attackCoroutine = PerformAttack();
        _attackPoint.StartCoroutine(_attackCoroutine);
    }

    public void OnExit()
    { }

    public void Tick()
    { }

    public bool IsDone() => _attackCoroutine == null;

    private IEnumerator PerformAttack()
    {
        yield return new WaitForSeconds(_delay);

        Collider bestTarget = _punchAssistant.GetColliders().Where(x => x.TryGetComponent(out IDamagable damagable))
            .OrderBy(x => Vector3.Distance(x.transform.position, _player.transform.position))
            .FirstOrDefault();

        if(bestTarget != null)
            _player.Movement.CharacterController.Rotate((bestTarget.transform.position - _player.transform.position).normalized);

        foreach (var collider in _attackPoint.GetColliders())
            if (collider.TryGetComponent(out IDamagable damagable) && collider.TryGetComponent(out Player player) == false)
                damagable.TakeDamage(_damage);

        yield return new WaitForSeconds(_cooldown);

        _attackCoroutine = null;
    }
}
