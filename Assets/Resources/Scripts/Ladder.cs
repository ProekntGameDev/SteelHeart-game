using UnityEngine;

public class Ladder : Interactable
{
    public Vector3 JumpOffForce => transform.TransformDirection(_jumpOffForce);

    [SerializeField] private Vector3 _jumpOffForce;
    [SerializeField] private float _ladderStepDistance;

    public Vector3 GetCenterOffset(Vector3 position)
    {
        Vector3 local = transform.worldToLocalMatrix.MultiplyPoint(position);
        local.z = 0;
        local.y = 0;

        return transform.TransformDirection(-local);
    }

    public Vector3 GetClosestLadder(Vector3 playerPosition)
    {
        Vector3 ladder = transform.TransformPoint(new Vector3(0, 
            Mathf.Round(transform.worldToLocalMatrix.MultiplyPoint(playerPosition).y / _ladderStepDistance) * _ladderStepDistance, 0));

        return ladder;
    }

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
