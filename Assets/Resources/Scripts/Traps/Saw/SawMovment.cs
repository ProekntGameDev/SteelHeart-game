using UnityEngine;

public class SawMovment : MonoBehaviour
{
    [SerializeField] float _sawMovementTime = 3;
    [SerializeField] Transform _ATransform;
    [SerializeField] Transform _BTransform;
    Vector3 _APos;
    Vector3 _BPos;

    float _startMovingFromPointTime;

    private void Start()
    {
        _APos = _ATransform.position;
        _BPos = _BTransform.position;

        _startMovingFromPointTime = Time.time;
    }

    private void FixedUpdate()
    {
        float delta = Mathf.Abs((Time.time - _startMovingFromPointTime) / _sawMovementTime);

        transform.localPosition = Vector3.Lerp(_APos, _BPos, delta);

        if (delta >= 1) _startMovingFromPointTime = Time.time + _sawMovementTime;
    }
}
