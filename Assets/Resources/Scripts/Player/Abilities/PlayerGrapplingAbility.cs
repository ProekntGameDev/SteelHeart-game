using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerGrapplingAbility : MonoBehaviour
{
    public Vector3 grapplingGunPosition = new Vector3 (0, 2f, 0);
    public float gizmoRadius = 0.25f;
    [Space]
    public float hook_mounting_distance_correction_speed = 0.2f;
    public float hook_mounting_target_distance = 4f;
    public float hook_grab_distance_max = 4f;
    public float airResistanceMultiplier = 0.99f;
    public float inputStrenght = 0.04f;
    public float gravityStrenght = -0.11f;
    [Space]
    public KeyCode grappleKey = KeyCode.Mouse0;
    public KeyCode ungrappleKey = KeyCode.Space;

    public bool isMounting { get; private set; } = false;

    [HideInInspector] public bool isHookUse_Allow = false;

    private PlayerMovement _movementController;
    private PlayerShootingAbility _shootingAbility;

    private float _delta_x = 0;
    private float _delta_y = 0;
    private float _rad_speed = 0;
    private Rigidbody _rigidbody;
    private Vector2 mount_point_delta;
    private Vector2 _hitted_object_position;
    private float _last_deltatime = 0;

    Collider selectedMountPoint = null;

    private void Awake()
    {
        _movementController = GetComponent<PlayerMovement>();
        _shootingAbility = GetComponent<PlayerShootingAbility>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        bool isOnFloor = _movementController.IsOnFloor;
        if (Input.GetKey(grappleKey) && isHookUse_Allow && isOnFloor == false)
        {
            float minDistance = float.MaxValue;
            Collider[] colliders = Physics.OverlapSphere(transform.position, hook_grab_distance_max);
            foreach (var collider in colliders)

                if (collider.transform.tag == "hook_mount_point")
                {
                    float distance = Vector3.Distance(transform.position, collider.transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance; selectedMountPoint = collider;
                    }
                }
        }

        if (selectedMountPoint != null || isMounting)
        {
            Grapple(selectedMountPoint);
            _movementController.isWalkingAllowed = false;
            if (_shootingAbility != null)
                _shootingAbility.enabled = false;
        }
        if (isMounting && Input.GetKey(ungrappleKey))
        {
            selectedMountPoint = null;
            Ungrapple();
            _movementController.isWalkingAllowed = true;
            if (_shootingAbility != null)
                _shootingAbility.enabled = true;
        }
    }

    public void Grapple(Collider collider)
    {
        if (_movementController == null) return;        

        float MoveControl_HorizontalAxis = Input.GetAxis("Horizontal");
        float rad;
        _delta_x = 0;
        _delta_y = 0;
        if (isMounting == false)
        {
            _rad_speed = 0;
            _rigidbody.velocity = Vector3.zero;
            _hitted_object_position = collider.transform.position;
            mount_point_delta = (Vector2)transform.position - _hitted_object_position;
        }
        // feature required state set^

        mount_point_delta = (Vector2)gameObject.transform.position - _hitted_object_position;

        float current_distance = Vector2.Distance(Vector2.zero, mount_point_delta);
        float distance_shift = current_distance - hook_mounting_target_distance;
        if (Mathf.Abs(distance_shift) > 0.01f)
        {
            _delta_x -= mount_point_delta.x * Mathf.Sign(distance_shift) * hook_mounting_distance_correction_speed * Time.deltaTime;
            _delta_y -= mount_point_delta.y * Mathf.Sign(distance_shift) * hook_mounting_distance_correction_speed * Time.deltaTime;
        }
        // distance correction^

        rad = Mathf.Atan2(mount_point_delta.y, mount_point_delta.x);
        _rad_speed *= airResistanceMultiplier;
        _rad_speed += inputStrenght * MoveControl_HorizontalAxis + gravityStrenght * Mathf.Cos(rad); ;
        float cos = Mathf.Cos(_rad_speed * Time.deltaTime);
        float sin = Mathf.Sin(_rad_speed * Time.deltaTime);
        _delta_x += mount_point_delta.x * cos - mount_point_delta.y * sin - mount_point_delta.x;
        _delta_y += mount_point_delta.x * sin + mount_point_delta.y * cos - mount_point_delta.y;
        gameObject.transform.position += Vector3.right * _delta_x + Vector3.up * _delta_y;
        // physics and user input control^

        _rigidbody.Sleep();
        _last_deltatime = Time.deltaTime;
        isMounting = true;
        _rigidbody.useGravity = false;
        // hook feature using^
        // restore state after hook feature using^
        // grapling hook ability^
    }

    public void Ungrapple()
    {
        _rigidbody.velocity = new Vector3(_delta_x / _last_deltatime, _delta_y / _last_deltatime);
        isMounting = false;
        _rigidbody.useGravity = true;//restore state
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 playerPosition = transform.position;
        Gizmos.DrawSphere(playerPosition + grapplingGunPosition, gizmoRadius);
    }
}
