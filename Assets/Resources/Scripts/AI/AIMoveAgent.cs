using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(CharacterController))]
public class AIMoveAgent : MonoBehaviour
{
    public float Speed { get { return NavMeshAgent.speed; } set { NavMeshAgent.speed = value; } }
    public float BaseOffset { get { return NavMeshAgent.baseOffset; } set { NavMeshAgent.baseOffset = value; } }
    public float StoppingDistance { get { return NavMeshAgent.stoppingDistance; } set { NavMeshAgent.stoppingDistance = value; } }
    public bool UpdateRotation { get { return NavMeshAgent.updateRotation; } set { NavMeshAgent.updateRotation = value; } }
    public bool IsStopped { get { return NavMeshAgent.isStopped; } set { NavMeshAgent.isStopped = value; } }

    public Vector3 Velocity { get; private set; }

    protected NavMeshAgent NavMeshAgent
    {
        get
        {
            if (_navMeshAgent == null)
                _navMeshAgent = GetComponent<NavMeshAgent>();

            return _navMeshAgent;
        }
    }

    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private float _friction = 0.2f;

    private Vector3 _destination;
    private NavMeshAgent _navMeshAgent;
    private CharacterController _characterController;

    public void SetVelocity(Vector3 velocity)
    {
        EnableController();

        Velocity = velocity;
    }

    public void SetDestination(Vector3 destination)
    {
        _destination = destination;

        if(_navMeshAgent.enabled)
            _navMeshAgent.destination = destination;
    }

    public void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (Vector3.Angle(Vector3.up, hit.normal) <= _characterController.slopeLimit)
        {
            Velocity = Vector3.zero;
            EnableAgent();
        }
    }

    public bool IsDone()
    {
        if (_navMeshAgent.enabled == false)
            return false;

        return _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance || _navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete;
    }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        if (_navMeshAgent.enabled || _characterController.enabled == false)
            return;

        _characterController.Move(Velocity * Time.fixedDeltaTime);

        Vector3 horizontalVelocity = GetHorizontalVelocity();
        float speed = horizontalVelocity.magnitude;

        if (_characterController.isGrounded && speed != 0)
        {
            float drop = speed * _friction * Time.fixedDeltaTime;
            horizontalVelocity *= Mathf.Max(speed - drop) / speed;

            Velocity = horizontalVelocity + Velocity.y * Vector3.up;
        }

        Velocity += _gravity * Time.fixedDeltaTime * Vector3.up;

        if (speed == 0 && _characterController.isGrounded)
            EnableAgent();
    }

    private void EnableAgent()
    {
        _navMeshAgent.enabled = true;

        _navMeshAgent.destination = _destination;
    }

    private void EnableController()
    {
        _navMeshAgent.enabled = false;
    }

    private Vector3 GetHorizontalVelocity() => new Vector3(Velocity.x, 0, Velocity.z);
}
