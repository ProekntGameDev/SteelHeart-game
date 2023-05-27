using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Interfaces;

public class Zipline : MonoBehaviour, IInteractableMonoBehaviour
{
    [FormerlySerializedAs("Speed (m/s)")] public float speed;
    [Space]
    public Transform point1;
    public Transform point2;

    private Vector3 _point1;
    private Vector3 _point2;

    private void Awake()
    {
        _point1 = point1.position;
        _point2 = point2.position;

        var collider1 = gameObject.AddComponent<SphereCollider>();
        var collider2 = gameObject.AddComponent<SphereCollider>();

        collider1.isTrigger = true;
        collider2.isTrigger = true;

        collider1.center = GetRelativePosition(collider1.transform, _point1);
        collider2.center = GetRelativePosition(collider2.transform, _point2);  
    }

    public void Interact(Transform obj)
    {
        if (obj.TryGetComponent(out PlayerMovement playerMovement))
            StartCoroutine(MoveCoroutine(playerMovement));
    }

    private IEnumerator MoveCoroutine(PlayerMovement playerMovement)
    {
        playerMovement.enabled = false;

        float time = 0;
        float timeDelta = speed / Vector3.Distance(_point1, _point2);
        Vector3 startPosition;
        Vector3 endPosition;

        if (Vector3.Distance(playerMovement.transform.position, _point1) < Vector3.Distance(playerMovement.transform.position, _point2))
        {
            startPosition = _point1;
            endPosition = _point2;
        }
        else
        {
            startPosition = _point2;
            endPosition = _point1;
        }

        Vector3 playerOffset = playerMovement.transform.position - startPosition;

        while (time <= 1)
        {
            playerMovement.transform.position = Vector3.Lerp(startPosition, endPosition, time) + playerOffset;
            time += timeDelta * Time.deltaTime;
            yield return null;
        }

        playerMovement.enabled = true;
    }

    private Vector3 GetRelativePosition(Transform origin, Vector3 position)
    {
        Vector3 distance = position - origin.position;
        Vector3 relativePosition = Vector3.zero;
        relativePosition.x = Vector3.Dot(distance, origin.right.normalized);
        relativePosition.y = Vector3.Dot(distance, origin.up.normalized);
        relativePosition.z = Vector3.Dot(distance, origin.forward.normalized);

        return relativePosition;
    }
}
