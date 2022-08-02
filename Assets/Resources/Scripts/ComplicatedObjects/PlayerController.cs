using System;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private Vector3 v3_left_rotation = new Vector3(0, 180, 0);
    private Vector3 v3_right_rotation = new Vector3(0, 0, 0);//front of model must be looking world-right
    //euler-angle values vectors^ constants^

    public KeyCode TimeSlowing_btn;
    public KeyCode NightVision_btn;
    // buttons^

    public float walk_accel = 200;
    public float max_walk_speed = 1;
    public float walk_damping_force = 200;//no_used
    public float jump_force = 200;
    public float climb_speed = 1;
    //
    public float block_req_stamina_level_for_activation;
    public float block_stamina_spend;
    public float sprint_walk_speed_scale;
    public float sprint_accel_scale;
    public float sprint_req_stamina_level_for_activation;//in points
    public float sprint_stamina_spend;//in points
    public float stamina_max;//point count
    public float stamina_restore_speed;//point per second
    //public float health_max;//in points
    public int shooting_damage = 5;
    public int additional_lives = 1;
    public int jetpack_jumps;
    public int jetpackJumpsLeft;
    // fields^

    private Rigidbody _rigidbody;
    private CapsuleCollider collider;
    // components^

    bool isNightvision_Allow = false;
    //
    private bool isOnFloor = true;
    private bool isSprinting = false;
    private bool isSneaking = false;
    private bool isTimeSlowed = false;
    private bool isBlocking = false;
    private bool isShooting = false;
    private bool isNightVisionActive = false;
    private bool isWalkingBanned = false;
    public bool IsJumpButtonPressed { get; private set; } = false;
    private bool isJumpButtonPressed_last = false;
    private bool isDownButtonPressed = false;
    [SerializeField] private Vector3 checkpoint;
    // object state^

    //
    private float stamina;
    //public float health;
    public float coins;
    // variables^

    BulletPool bullet_pool;
    //temporary fields^

    private Health _health;

    private void Awake()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        _rigidbody.sleepThreshold = 0;//rigidbody never sleep now

        collider = gameObject.GetComponent<CapsuleCollider>();

        stamina = stamina_max;

        checkpoint = gameObject.transform.position;

        bullet_pool = new BulletPool(100);

        _health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        _health.OnDeath += Death;
    }

    private void OnDisable()
    {
        _health.OnDeath -= Death;
    }

    private void FixedUpdate()
    {
        float MoveControl_HorizontalAxis = Input.GetAxis("Horizontal");
        bool isMoveLeft = MoveControl_HorizontalAxis < 0;
        bool isMoveRight = MoveControl_HorizontalAxis > 0;
        //
        Vector3 cursor_delta = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        if (isShooting)
        {
            if (cursor_delta.x < 0) transform.eulerAngles = v3_left_rotation;
            else if (cursor_delta.x > 0) transform.eulerAngles = v3_right_rotation;
        }
        else
        {
            if (isMoveLeft) transform.eulerAngles = v3_left_rotation;
            else if (isMoveRight) transform.eulerAngles = v3_right_rotation;
        }
        if (isWalkingBanned == false)
            Walk(walk_accel * MoveControl_HorizontalAxis);//front of model must be looking world-right
        // walk ability^


        bool isSprintButtonPressed = Input.GetKey(KeyCode.LeftShift);
        bool isStaminaEnough = stamina >= sprint_req_stamina_level_for_activation;
        if (isSprintButtonPressed && isStaminaEnough)
        {
            if (isSprinting == false)
            {
                max_walk_speed *= sprint_walk_speed_scale;
                walk_accel *= sprint_accel_scale;
            }
            stamina -= sprint_stamina_spend * Time.fixedDeltaTime;
            isSprinting = true;
        }
        else if (isSprinting == true && isOnFloor == true)
        {
            max_walk_speed *= 1 / sprint_walk_speed_scale;
            walk_accel *= 1 / sprint_accel_scale;
            isSprinting = false;
            //restore state after sprint ability using^
        }
        // sprint ability^

        isDownButtonPressed = Input.GetAxis("Vertical") < 0;
        if (isDownButtonPressed && isOnFloor && isSneaking == false) { collider.height /= 2; isSneaking = true; }
        else if (isSneaking && isDownButtonPressed == false) { collider.height *= 2; isSneaking = false; }
        // sneaking ability^


        if (Input.GetKey(TimeSlowing_btn) && isTimeSlowed == false)
        { Time.timeScale /= 2; isTimeSlowed = true; }
        else if (isTimeSlowed)
        { Time.timeScale *= 2; isTimeSlowed = false; }
        // time slowing ability^

        if (Input.GetKey(NightVision_btn) && isNightvision_Allow)
        {
            isNightVisionActive = !isNightVisionActive;
            Camera.main.gameObject.GetComponent<CameraController>().NightVisionEffectActiveStateChange();
        }
        // night vision ability^

        bullet_pool.Tick();
        if (Input.GetMouseButton(0))
        {
            Vector3 bullet_spawnpoint = gameObject.transform.position + Vector3.right * Math.Sign(cursor_delta.x);
            Vector3 wp = Camera.main.WorldToScreenPoint(bullet_spawnpoint);
            bullet_pool.UseBullet(shooting_damage, 1, Vector3.Normalize(Input.mousePosition - wp) * 6, bullet_spawnpoint);
            isShooting = true;
        }
        else isShooting = false;
        // shoot ability^

        if (Input.GetMouseButton(1))
        {
            isBlocking = stamina > block_req_stamina_level_for_activation;
            stamina -= block_stamina_spend * Time.deltaTime;
        }
        else isBlocking = false;
        // block ability^

        if (Input.GetAxis("Submit") > 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "checkpoint")
                    checkpoint = hit.collider.transform.position - Vector3.forward * hit.collider.transform.position.z;
                if (hit.collider.gameObject.tag == "restoration_point")
                    _health.FullHeal();
            }
        }
        // clickable object using ability^

        if (isSprintButtonPressed == false) 
            stamina = (stamina > stamina_max) ? stamina_max : stamina + stamina_restore_speed * Time.fixedDeltaTime;
        // restorable restoration^
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.gameObject.tag == "climbing_wall")
        {
            if (IsJumpButtonPressed) Climb(climb_speed);
            isOnFloor = false;
            //ray_lenght = 0;
        }
        //climbing_wall feature^

        if (collision.collider.gameObject.tag == "fragile")
        {
            collision.collider.gameObject.GetComponent<FragilePlatform>().Tick(Time.deltaTime);
        }
        //breaking fragiles feature^
    }

    private void OnCollisionExit(Collision collision)
    {
        //if (collision.collider.gameObject.tag == "climbing_wall") ray_lenght = ray_lenght_default;
    }

    private void OnTriggerEnter(Collider trigger)
    {
        var interactable = trigger.GetComponent<IInteractableMonoBehaviour>();
        if (interactable != null)
        {
            interactable.Interact(this.transform);
        }

        if (trigger.gameObject.tag == "drag_object")
        {
            gameObject.transform.position = trigger.gameObject.transform.position;

            _rigidbody.useGravity = false;
            _rigidbody.velocity = Vector3.zero;
            isWalkingBanned = true;
            isOnFloor = false;
        }
        //zipline/crane feature^

        if (trigger.gameObject.tag == "ladder")
        {
            if (IsJumpButtonPressed) Climb(climb_speed);
            if (isDownButtonPressed) Climb(-climb_speed);

            _rigidbody.useGravity = false;
            _rigidbody.velocity -= _rigidbody.velocity.y * Vector3.up;
            isOnFloor = false;
            //ray_lenght = 0;
        }
        //ladder feature^

        if (trigger.gameObject.tag == "upgrade_nightvision")
        {
            isNightvision_Allow = true;
            trigger.gameObject.SetActive(false);
        }
        //nightvision feature^
    }
    private void OnTriggerExit(Collider trigger)
    {
        if (trigger.gameObject.tag == "ladder") 
        { 
            _rigidbody.useGravity = true; 
            //ray_lenght = ray_lenght_default; 
        }
        //restore state after ladder feature using^
        if (trigger.gameObject.tag == "drag_object") { _rigidbody.useGravity = true; isWalkingBanned = false; }
        //restore state after crane feature using^
    }

    private void Walk(float speed)
    {
        _rigidbody.AddForce(Vector3.right * speed, ForceMode.Acceleration);
        float direction = (_rigidbody.velocity.x >= 0) ? 1 : -1;
        if (Math.Abs(_rigidbody.velocity.x) > max_walk_speed) _rigidbody.velocity = _rigidbody.velocity + Vector3.right * (max_walk_speed * direction - _rigidbody.velocity.x);
    }

    /*
    private void Jump(float force)
    {
        _rigidbody.velocity += Vector3.up * force;
        isOnFloor = false;
    }
    */

    private void Climb(float speed)
    {
        gameObject.transform.position += gameObject.transform.up * speed * Time.fixedDeltaTime;
    }

    public void Death()
    {
        if (additional_lives > 0)
        {
            additional_lives -= 1;
            Respawn();
        }
        else Debug.Log("Death!");
    }

    private void Respawn()
    {
        gameObject.transform.position = checkpoint;
        _health.FullHeal();
    }

    /*
    private bool CheckFloor()
    {
        return Physics.Raycast(transform.position, Vector3.down, floorCheckRayLength);
    }
    */
}
