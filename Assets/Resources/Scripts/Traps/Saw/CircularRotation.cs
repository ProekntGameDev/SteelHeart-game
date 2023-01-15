using UnityEngine;

public class CircularRotation : MonoBehaviour
{
    [SerializeField] float _rotationSpeed = 12;
    Vector3 _rotationDirection;

    private void Start() => _rotationDirection = Vector3.forward * _rotationSpeed;
    private void FixedUpdate() => transform.localRotation *= Quaternion.Euler(_rotationDirection);
}
