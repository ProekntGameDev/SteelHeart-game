using UnityEngine;

public class AttackPoint : MonoBehaviour
{
    public float Radius => _radius;

    [SerializeField] private float _radius;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
