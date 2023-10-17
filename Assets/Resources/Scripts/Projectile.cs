using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _lifetime;

    private Rigidbody _rigidbody;
    private Vector3 _direction;
    private Health _owner;
    private float _damage;

    public void Init(Vector3 direction, float damage, Health owner)
    {
        _direction = direction;
        _damage = damage;
        _owner = owner;

        _rigidbody.velocity = _direction * _speed;

        StartCoroutine(Lifetime());
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;

        if (other.TryGetComponent(out IDamagable damagable))
            damagable.TakeDamage(new Damage(_damage, false, false), _owner);

        Destroy(gameObject);
    }

    private IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(_lifetime);
        Destroy(gameObject);
    }
}
