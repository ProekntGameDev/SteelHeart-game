using System.Collections.Generic;
using UnityEngine;

public class AttackPoint : MonoBehaviour
{
    public float Radius => _radius;

    [SerializeField] private float _radius;

    public IEnumerable<Collider> GetColliders()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _radius, Physics.AllLayers, QueryTriggerInteraction.Ignore);

        foreach (var collider in colliders)
            yield return collider;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
