using System;
using UnityEngine;

public class Enemy : MonoBehaviour
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

    private bool isOnFloor = false;
    private bool isBufferFilled = false;
    private bool isSprinting = false;
    private bool isPlayerDetected = false;
    // object state^

    float health;
    float damage;
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
    //
    Vector3 buffer_position;
    public Vector3 target_position;
    Transform player_transform;
    float patrol_direction = 1;
    Vector3 next_patrol_position;
    //
    delegate void MoveDamping();
    MoveDamping move_damping;
    delegate void Movement();
    Movement move;
    // variables^

    private Vector3 v3_left_rotation = new Vector3(0, 180, 0);
    private Vector3 v3_right_rotation = new Vector3(0, 0, 0);//front of model must be looking world-right
    //euler-angle values vectors^ constants^
    Ray ray;
    //supply variables^

    void Start()
    {
        physics_component = gameObject.GetComponent<Rigidbody>();
        bullet_pool = gameObject.GetComponent<BulletPool>();
        player_transform = GameObject.Find("Player").transform;
        target_position = next_patrol_position = gameObject.transform.position;

        switch (type) {
            case EnemyTypes.WalkerDrone: 
                health = 100; damage = 15; range_of_view = 9; speed = 2; accel = 100; patrol_distance = 9; move = Walk;
                sprint_speed_scale = 2f; sprint_accel_scale = 2f; shooting_distance = 9; shooting_cooldown = 1f; physics_component.useGravity = true;
                break;
            case EnemyTypes.HelicopterDrone: 
                health = 25; damage = 25; range_of_view = 16; speed = 2; accel = 5; move = Fly; fly_height = 4f; patrol_distance = 9; physics_component.useGravity = false;
                sprint_speed_scale = 2f; sprint_accel_scale = 2f; shooting_distance = 14; shooting_cooldown = 0.3f;
                break;
            default: 
                break;
        }
        if (isElite) 
        {
            health *= 2;
            damage *= 2;
        }
    }

    void Update()
    {
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
        //sprint ability^

        if (isPlayerDetected)
        {
            buffer_position = target_position;
            target_position = player_transform.position;
            isBufferFilled = true;
        }
        else 
        {
            if (isBufferFilled)
            {
                target_position = buffer_position; 
                isBufferFilled = false;
            }
            if (Math.Abs(gameObject.transform.position.x-target_position.x) < 1f) 
            {
                next_patrol_position += Vector3.right * patrol_direction * patrol_distance;
                patrol_direction *= -1;
                target_position = next_patrol_position;
            }
        }
        // target change^
        move();

        transform.eulerAngles = (Math.Sign(physics_component.velocity.x) > 0 && Math.Sign(physics_component.velocity.x) != 0) ? v3_right_rotation : v3_left_rotation;

        if (isPlayerDetected && Vector3.Distance(gameObject.transform.position, player_transform.position) < shooting_distance)
        {
            if (shooting_cooldown_timer < 0) { shooting_cooldown_timer = shooting_cooldown; bullet_pool.UseBullet(5, 1, (player_transform.position - gameObject.transform.position).normalized * 20f, gameObject.transform.position); }
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
            physics_component.velocity = physics_component.velocity + Vector3.right * (speed * Math.Sign(physics_component.velocity.x) - physics_component.velocity.x);
    }
    private void Fly()
    {
        float accel_mult = (target_position + (Vector3.up * fly_height) - gameObject.transform.position).magnitude / range_of_view;
        Vector3 direction = (target_position + (Vector3.up * fly_height) - gameObject.transform.position).normalized;
        physics_component.velocity = direction * accel * accel_mult * Time.deltaTime;
        float velocity_magnitude = physics_component.velocity.magnitude;
        if (velocity_magnitude > speed) physics_component.velocity *= speed/velocity_magnitude;//comment this for get orbital satellite
        if (accel_mult < 0.1f) physics_component.velocity *= 1f - Time.deltaTime;
    }
}
