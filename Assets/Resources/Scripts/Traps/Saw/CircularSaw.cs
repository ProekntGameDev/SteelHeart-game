using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CircularSaw : MonoBehaviour
{
    [SerializeField] float _baseDamage = 1f;
    [SerializeField] float _damageIncrease = 0.002f;
    [SerializeField] float _rotationSpeed = 12;
    [SerializeField] LayerMask _ignoredLayers;

    bool OnSaw;
    float _playerTimeEnterDamageSourse;
    Vector3 _rotationDirection;
    IDamagable _target;

    public float Damage { get { return _baseDamage + (_damageIncrease * (Time.time - _playerTimeEnterDamageSourse)); } }

    private void Start()
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;
        _rotationDirection = Vector3.forward * _rotationSpeed;
    }
    private void FixedUpdate() => transform.localRotation *= Quaternion.Euler(_rotationDirection);

    private void OnTriggerEnter(Collider other)
    {
        if (_ignoredLayers == (_ignoredLayers | 1 << other.gameObject.layer)) return;
        MakeDamage(other);
    }
    private void OnTriggerExit(Collider other)
    {
        if (_ignoredLayers == (_ignoredLayers | 1 << other.gameObject.layer)) return;
        OnSaw = false;
        _target = null;
    }
    private void MakeDamage(Collider target)
    {
        if (target.TryGetComponent(out IDamagable damagable))
        {
            _target = damagable;
            _playerTimeEnterDamageSourse = Time.time;
            OnSaw = true;
            StartCoroutine(PeriodicDamageCalculationCoroutine());
        }
    }

    public IEnumerator PeriodicDamageCalculationCoroutine()
    {
        while (OnSaw)
        {
            _target.TakeDamage(Damage);
            yield return null;
        }
    }
}
