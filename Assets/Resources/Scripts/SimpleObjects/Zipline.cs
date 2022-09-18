using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class Zipline : MonoBehaviour, IInteractableMonoBehaviour
{
    [FormerlySerializedAs("Speed (m/s)")] public float speed;
    [Space]
    public Transform point1;
    public Transform point2;

    private PlayerMovement _playerMovement;
    private PlayerJump _playerJump;
    private PlayerCrouch _playerCrouch;
    private Rigidbody _rigidbody;

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

        collider1.center = getRelativePosition(collider1.transform, _point1);
        collider2.center = getRelativePosition(collider2.transform, _point2);  
    }

    public void Interact(Transform obj)
    {
        _playerMovement = obj.GetComponent<PlayerMovement>();
        if (_playerMovement == null) return;

        _playerJump = obj.GetComponent<PlayerJump>();
        if (_playerJump == null) return;

        _playerCrouch = obj.GetComponent<PlayerCrouch>();
        if (_playerCrouch == null) return;

        _rigidbody = obj.GetComponent<Rigidbody>();

        _playerMovement.enabled = false;
        _playerJump.enabled = false;
        _playerCrouch.enabled = false;

        StartCoroutine(MoveCoroutine());
    }

    private IEnumerator MoveCoroutine()
    {
        _playerMovement.enabled = false;
        _playerJump.enabled = false;
        _playerCrouch.enabled = false;

        float time = 0;
        float timeDelta = speed / Vector3.Distance(_point1, _point2);
        Vector3 startPosition;
        Vector3 endPosition;

        if (Vector3.Distance(_playerMovement.transform.position, _point1) < Vector3.Distance(_playerMovement.transform.position, _point2))
        {
            startPosition = _point1;
            endPosition = _point2;
        }
        else
        {
            startPosition = _point2;
            endPosition = _point1;
        }

        Vector3 playerOffset =_playerMovement.transform.position - startPosition;

        while (time <= 1)
        {
            _playerMovement.transform.position = Vector3.Lerp(startPosition, endPosition, time) + playerOffset;
            time += timeDelta * Time.deltaTime;
            yield return null;
        }

        _playerMovement.enabled = true;
        _playerJump.enabled = true;
        _playerCrouch.enabled = true;

        if (_rigidbody != null)
        {
            var velocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);
            _rigidbody.velocity = velocity;
        }
    }

    private Vector3 getRelativePosition(Transform origin, Vector3 position)
    {
        Vector3 distance = position - origin.position;
        Vector3 relativePosition = Vector3.zero;
        relativePosition.x = Vector3.Dot(distance, origin.right.normalized);
        relativePosition.y = Vector3.Dot(distance, origin.up.normalized);
        relativePosition.z = Vector3.Dot(distance, origin.forward.normalized);

        return relativePosition;
    }
}
