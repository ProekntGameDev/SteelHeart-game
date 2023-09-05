using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class MineRadar : MonoBehaviour
{
    public delegate void MineRadarEvent(float activationTime);
    public event MineRadarEvent OnTargetEnter;

    [SerializeField] float _timeToExplosion = 2f;
    [SerializeField] float _activationRadius = 1.5f;

    private void Awake()
    {
        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = _activationRadius;
        sphereCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Health>(out var target))
        {
            OnTargetEnter?.Invoke(_timeToExplosion);
            gameObject.SetActive(false);
        }
    }
}
