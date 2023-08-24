using UnityEngine;

public class Ladder : MonoBehaviour
{
    public Vector3 JumpOffForce => transform.TransformDirection(_jumpOffForce);

    [SerializeField] private Vector3 _jumpOffForce;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * 2);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawRay(transform.position, JumpOffForce);
    }
}
