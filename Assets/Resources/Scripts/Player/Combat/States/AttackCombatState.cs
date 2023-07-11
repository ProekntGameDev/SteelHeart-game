using UnityEngine;

public class AttackCombatState : IState
{
    private AttackPoint _attackPoint;
    private float _speed;
    private float _damage;
    private float _timestamp;

    public AttackCombatState(AttackPoint attackPoint, float speed, float damage)
    {
        _attackPoint = attackPoint;
        _speed = speed;
        _damage = damage;
    }

    public void OnEnter()
    {
        _timestamp = Time.time;
    }

    public void OnExit()
    {
        Collider[] colliders = Physics.OverlapSphere(_attackPoint.transform.position, _attackPoint.Radius);

        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent(out IDamagable damagable) && collider.TryGetComponent(out Player player) == false)
            {
                Debug.Log($"Damage to {collider.name}");
                damagable.TakeDamage(_damage);
            }
        }
    }

    public void Tick()
    { }

    public bool IsDone() => Time.time >= _timestamp + (1 / _speed);
}
