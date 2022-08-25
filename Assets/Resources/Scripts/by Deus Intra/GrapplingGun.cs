using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Rigidbody))]
public class GrapplingGun : MonoBehaviour
{
    public Vector3 gunPosition;
    public float gizmoRadius = 1f;

    [Space]
    public float hook_mounting_distance_correction_speed = 0.2f;
    public float hook_mounting_target_distance = 4f;
    public float airResistanceMultiplier = 0.99f;

    [HideInInspector] public bool isHookUse_Allow = false;
    public bool isMounting { get; private set; } = false;

    private PlayerController _playerController;

    private float _delta_x = 0;
    private float _delta_y = 0;
    private float _rad_speed = 0;
    private Rigidbody _rigidbody;
    private Vector2 mount_point_delta;
    private Vector2 _hitted_object_position;
    private float _last_deltatime = 0;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Grapple()
    {
        if (_playerController == null) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        float MoveControl_HorizontalAxis = Input.GetAxis("Horizontal");

        bool isHookMountingPointHit = Physics.Raycast(ray, out hit) && hit.collider.gameObject.tag == "hook_mount_point";

            float rad;
            _delta_x = 0;
            _delta_y = 0;
            if (isMounting == false)
            {
                _rad_speed = 0;
                _rigidbody.velocity = Vector3.zero;
                _hitted_object_position = hit.collider.gameObject.transform.position;
                mount_point_delta = (Vector2)gameObject.transform.position - _hitted_object_position;
            }
            // feature required state set^

            mount_point_delta = (Vector2)gameObject.transform.position - _hitted_object_position;

            float current_distance = Vector2.Distance(Vector2.zero, mount_point_delta);
            float distance_shift = current_distance - hook_mounting_target_distance;
            Debug.Log(distance_shift);
            if (Mathf.Abs(distance_shift) > 0.01f)
            {
                _delta_x -= mount_point_delta.x * Mathf.Sign(distance_shift) * hook_mounting_distance_correction_speed * Time.deltaTime;
                _delta_y -= mount_point_delta.y * Mathf.Sign(distance_shift) * hook_mounting_distance_correction_speed * Time.deltaTime;
            }
            // distance correction^

            rad = Mathf.Atan2(mount_point_delta.y, mount_point_delta.x);
            _rad_speed *= airResistanceMultiplier;
            _rad_speed += 0.04f * MoveControl_HorizontalAxis + -0.27f * Mathf.Cos(rad); ;
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
        Gizmos.DrawSphere(playerPosition + gunPosition, gizmoRadius);
    }
}
