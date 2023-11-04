using UnityEngine;

public partial class PlayerCharacterController : MonoBehaviour
{
    public float Height => _characterController.height;
    public float SlopeLimit => _characterController.slopeLimit;

    [SerializeField] private float _rotationTime = 0.2f;

    public CollisionFlags Move()
    {
        return _characterController.Move(CurrentVelocity * Time.fixedDeltaTime);
    }

    public CollisionFlags Move(Vector3 motion, bool updateGrounded = false)
    {
        CollisionFlags collisionFlags = _characterController.Move(motion);
        //_currentVelocity = _characterController.velocity;

        if (updateGrounded)
            IsGrounded = _characterController.isGrounded;

        return collisionFlags;
    }

    public void SetPosition(Vector3 newPosition)
    {
        _characterController.enabled = false;
        transform.position = newPosition;
        _characterController.enabled = true;
    }

    public bool CanSetHeight(float height)
    {
        float heightDifference = (height - _characterController.height) * 1.25f;

        if (heightDifference <= 0)
            return true;

        Vector3 origin = new Vector3(transform.position.x, _characterController.bounds.max.y, transform.position.z);
        Ray ray = new Ray(origin, Vector3.up);

        Debug.DrawLine(ray.origin, ray.origin + ray.direction * heightDifference, Color.red, 15f);

        if (Physics.Raycast(ray, heightDifference, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            return false;

        return true;
    }

    public void SetHeight(float height)
    {
        float oldHeight = _characterController.height;
        _characterController.height = height;
        _characterController.center += new Vector3(0, (height - oldHeight) / 2, 0);
    }

    public void Rotate(Vector3 forward)
    {
        if (forward.sqrMagnitude == 0)
            return;

        transform.forward = Vector3.Slerp(transform.forward, forward, _rotationTime);
    }

    public void Rotate(Vector3 forward, float slerpTime)
    {
        if (forward.sqrMagnitude == 0)
            return;

        transform.forward = Vector3.Slerp(transform.forward, forward, slerpTime);
    }

    public Vector2 ReadInputAxis() => _player.Input.Player.Axis.ReadValue<Vector2>();
}
