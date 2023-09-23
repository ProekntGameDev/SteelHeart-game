using UnityEngine;

public partial class InertialCharacterController : MonoBehaviour
{
    public float Height => _characterController.height;
    public float SlopeLimit => _characterController.slopeLimit;

    [SerializeField] private float _friction = 0.2f;
    [SerializeField] private float _rotationTime = 0.2f;

    public CollisionFlags GroundMove(Vector3 wishDirection, float accelerate, float maxVelocity)
    {
        if (IsGrounded == false)
            throw new System.InvalidOperationException();

        float speed = _currentVelocity.magnitude;

        //Apply friction
        if (_verticalVelocity <= 0 && speed != 0)
        {
            float drop = speed * _friction * Time.fixedDeltaTime;
            _currentVelocity *= Mathf.Max(speed - drop, 0f) / speed;
        }

        Accelerate(wishDirection, accelerate, maxVelocity);

        //Slope movement
        Vector3 slopeMovement = _currentVelocity.magnitude * Vector3.down * (GetSlopeAngle() / 45);

        CollisionFlags collisionFlags = Move((_currentVelocity + slopeMovement) * Time.fixedDeltaTime);

        return collisionFlags;
    }

    public CollisionFlags AirMove(Vector3 wishDirection, float accelerate, float maxVelocity)
    {
        if(IsGrounded)
            throw new System.InvalidOperationException();

        Accelerate(wishDirection, accelerate, maxVelocity);

        return Move(_currentVelocity * Time.fixedDeltaTime);
    }

    public CollisionFlags Move(Vector3 motion, bool updateGrounded = false)
    {
        CollisionFlags collisionFlags = _characterController.Move(motion);

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

    public Vector2 ReadInputAxis() => _player.Input.Player.Axis.ReadValue<Vector2>();

    private void Accelerate(Vector3 accelerateDirection, float accelerate, float maxVelocity)
    {
        //Ignore vertical component of wish direction
        Vector3 alignedInputVelocity = new Vector3(accelerateDirection.x * accelerate, 0f, accelerateDirection.z * accelerate) * Time.deltaTime;

        //Get current velocity
        Vector3 currentVelocity = new Vector3(_currentVelocity.x, 0f, _currentVelocity.z);

        //How close the current speed to max velocity is (1 = not moving, 0 = at/over max speed)
        float max = Mathf.Max(0f, 1 - (currentVelocity.magnitude / maxVelocity));

        //How perpendicular the input to the current velocity is (0 = 90°)
        float velocityDot = Vector3.Dot(currentVelocity, alignedInputVelocity);

        //Scale the input to the max speed
        Vector3 modifiedVelocity = alignedInputVelocity * max;

        //The more perpendicular the input is, the more the input velocity will be applied
        Vector3 correctVelocity = Vector3.Lerp(alignedInputVelocity, modifiedVelocity, velocityDot);

        _currentVelocity += correctVelocity;
    }
}
