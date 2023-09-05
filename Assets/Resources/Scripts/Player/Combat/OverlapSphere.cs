using System.Collections.Generic;
using UnityEngine;

public class OverlapSphere : MonoBehaviour
{
    public float Radius => _radius;

    [SerializeField] private float _radius;
    [SerializeField] private QueryTriggerInteraction _triggerInteraction = QueryTriggerInteraction.Ignore;
    [SerializeField] private Color _gizmosColor = Color.green;

    public IEnumerable<Collider> GetColliders()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _radius, Physics.AllLayers, _triggerInteraction);

        foreach (var collider in colliders)
            yield return collider;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = _gizmosColor;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
