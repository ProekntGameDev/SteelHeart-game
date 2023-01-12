using UnityEngine;

public class SawMovment : MonoBehaviour
{
    [SerializeField] float _sawMovementTime = 3;
    [SerializeField] float _movementDist = 4;
    Vector3 _APos;
    Vector3 _BPos;

    float _startMovingFromPointTime;

    private void Start()
    {
        Vector3 dirOnRight = transform.right;
        dirOnRight.Normalize();
        _APos = _BPos = transform.localPosition;
        _BPos -= dirOnRight * _movementDist;

        _startMovingFromPointTime = Time.time;
    }

    private void FixedUpdate()
    {
        float delta = Mathf.Abs((Time.time - _startMovingFromPointTime) / _sawMovementTime);

        transform.localPosition = Vector3.Lerp(_APos, _BPos, delta);

        if (delta > 1) _startMovingFromPointTime = Time.time + _sawMovementTime;
        if (delta < 0) _startMovingFromPointTime = Time.time;
    }
}
