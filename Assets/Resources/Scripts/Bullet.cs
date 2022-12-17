using UnityEngine;
using Interfaces;

public class Bullet : MonoBehaviour, ITriggerableMonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _lifetime;

    private float _currentLifetime;


    public void Trigger(Transform obj)
    {
        var blockAblility = obj.GetComponent<PlayerBlockAbility>();
        if (blockAblility != null)
            if (blockAblility.IsBlocking) return;

        var health = obj.GetComponent<HealthOld>();
        if (health == null) return;

        health.Damage(_damage);
        gameObject.SetActive(false);
    }

    public void Tick()
    {
        if (_currentLifetime < 0) gameObject.SetActive(false);
        else _currentLifetime -= Time.fixedDeltaTime;
    }

    public void Shoot()
    {
        _currentLifetime = _lifetime;
    }
}
