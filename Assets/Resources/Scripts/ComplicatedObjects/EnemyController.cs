using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private BulletPool bullet_pool;
    private Rigidbody physics_component;
    // components^

    public enum EnemyTypes
    {
        WalkerDrone,
        HelicopterDrone
    }
    public EnemyTypes type;
    public bool isElite = false;
    // fields^

    //private bool isOnFloor = false;
    private bool isBufferFilled = false;
    private bool isSprinting = false;
    private bool isPlayerDetected = false;
    // object state^
    public float health;
    int damage;
    float attack_cooldown;
    float shooting_cooldown;
    float shooting_cooldown_timer = -1;
    float range_of_view;
    float patrol_distance;
    float shooting_distance;
    float speed;
    float accel;
    float move_damping_speed;
    float fly_height;
    float sprint_speed_scale;
    float sprint_accel_scale;
    float reachtarget_waiting;
    float waiting_counter = 0;
    //
    Vector3 buffer_position;
    Vector3 target_position;
    Transform player_transform;
    float patrol_direction = 1;
    Vector3 next_patrol_position;
    Vector3 start_position;
    Vector3 shooting_offset;
    //
    delegate void MoveDamping();
    MoveDamping move_damping;
    delegate void Movement();
    Movement move;
    // variables^

    private Vector3 v3_left_rotation = new Vector3(0, 180, 0);
    private Vector3 v3_right_rotation = new Vector3(0, 0, 0);//front of model must be looking world-right
    //euler-angle values vectors^ constants^

    void Start()
    {
        player_transform = FindObjectOfType<PlayerMovement>().transform;
        physics_component = gameObject.GetComponent<Rigidbody>();
        bullet_pool = new BulletPool(10);

        switch (type) {
            case EnemyTypes.WalkerDrone: 
                health = 100; damage = 15; range_of_view = 9; speed = 2; accel = 100; patrol_distance = 9; move = Walk; shooting_offset = Vector3.up*-1f;
                 sprint_speed_scale = 2f; sprint_accel_scale = 2f; shooting_distance = 9; shooting_cooldown = 1f; physics_component.useGravity = true; reachtarget_waiting = 4f;
                break; 
            case EnemyTypes.HelicopterDrone: 
                health = 25; damage = 25; range_of_view = 12; speed = 2; accel = 5; move = Fly; fly_height = 4f; patrol_distance = 9; physics_component.useGravity = false;
                sprint_speed_scale = 2f; sprint_accel_scale = 2f; shooting_distance = 14; shooting_cooldown = 0.3f; reachtarget_waiting = 10f; shooting_offset = Vector3.up * 0.6f;
                break;
            default: 
                break;
        }
        if (isElite) 
        {
            health *= 2;
            damage *= 2;
        }

        Ray ray = new Ray(gameObject.transform.position, -gameObject.transform.up);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        gameObject.transform.position += (gameObject.transform.position.y - hit.point.y < fly_height) ? Vector3.up*(hit.point.y + fly_height - gameObject.transform.position.y) : Vector3.zero;
        start_position = target_position = next_patrol_position = gameObject.transform.position;
    }

    void Update()
    {
        if (health <= 0) gameObject.SetActive(false);
        // death ability^

        isPlayerDetected = Vector3.Distance(gameObject.transform.position, player_transform.position) < range_of_view;
        if (isPlayerDetected)
        {
            if (isSprinting == false)
            {
                speed *= sprint_speed_scale;
                accel *= sprint_accel_scale;
            }
            isSprinting = true;
        }
        else if (isSprinting == true)
        {
            speed *= 1/sprint_speed_scale;
            accel *= 1/sprint_accel_scale;
            isSprinting = false;
            //restore state after sprint ability using^
        }
        // sprint ability^

        if (isPlayerDetected)
        {
            buffer_position = target_position;
            target_position = player_transform.position;
            isBufferFilled = true;
        }
        else 
        {
            if (waiting_counter > 0) { waiting_counter -= Time.deltaTime; }
            else
            {
                waiting_counter = reachtarget_waiting;
                if (isBufferFilled)
                {
                    target_position = buffer_position;
                    isBufferFilled = false;
                }
                if (Math.Abs(gameObject.transform.position.x - target_position.x) < 1f)
                {
                    next_patrol_position += Vector3.right * patrol_direction * patrol_distance;
                    patrol_direction *= -1;
                    target_position = next_patrol_position;
                }
            }
        }
        // target change^

        move();
        // moving to target^

        transform.eulerAngles = (Math.Sign(physics_component.velocity.x) > 0 && Math.Sign(physics_component.velocity.x) != 0) ? v3_right_rotation : v3_left_rotation;
        // update model face direction^

        if (isPlayerDetected && Vector3.Distance(gameObject.transform.position, player_transform.position) < shooting_distance)
        {
            if (shooting_cooldown_timer < 0) { shooting_cooldown_timer = shooting_cooldown; bullet_pool.UseBullet(/*damage, 1,*/ (player_transform.position + shooting_offset - gameObject.transform.position).normalized * 20f, gameObject.transform.position - shooting_offset); }
            else shooting_cooldown_timer -= Time.deltaTime;
        }
        else shooting_cooldown_timer = -1;
        // shooting ability^
    }

    private void Walk()
    {
        Vector3 direction = (target_position - gameObject.transform.position).normalized;
        physics_component.velocity += Vector3.right * Math.Sign(direction.x) * accel * Time.deltaTime;
        if (Math.Abs(physics_component.velocity.x) > speed)
            physics_component.velocity = physics_component.velocity + Vector3.right * (speed * Math.Sign(physics_component.velocity.x) - physics_component.velocity.x);//velocity limit
    }
    private void Fly()
    {
        Vector3 offset = (isPlayerDetected) ? Vector3.up * fly_height : Vector3.zero;
        Vector3 direction = target_position + offset - gameObject.transform.position;
        float accel_multiplier = direction.magnitude / range_of_view;
        physics_component.velocity += direction.normalized * accel * accel_multiplier * Time.deltaTime;
        float velocity_magnitude = physics_component.velocity.magnitude;
        if (velocity_magnitude > speed) physics_component.velocity *= speed/velocity_magnitude;//velocity limit
        if (accel_multiplier < 0.1f) physics_component.velocity *= 1f - Time.deltaTime;//velocity damping on approach target
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "explosive")
        {
            collider.gameObject.GetComponent<Explosive>().DamageSubscribe(this);
        }
    }
    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "explosive")
        {
            collider.gameObject.GetComponent<Explosive>().CancelDamageSubscription(this);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.gameObject.tag == "fragile")
        {
            //collision.collider.gameObject.GetComponent<FragilePlatform>().Tick(Time.deltaTime);
        }
        //breaking fragiles feature^
    }
}
