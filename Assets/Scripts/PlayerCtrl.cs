using System;
using UnityEngine;

/*
WARNING!!! ALARM!!!
front of model must be looking world-right!
WARNING!!! ALARM!!!
*/
public class PlayerCtrl : MonoBehaviour
{
    private Vector3 v3_left_rotation = new Vector3(0, 180, 0);
    private Vector3 v3_right_rotation = new Vector3(0, 0, 0);//front of model must be looking world-right
    //euler-angle values vectors^ constants^

    public float walk_accel = 200;
    public float max_walk_speed = 1;
    public float walk_damping_force = 200;//no_used
    public float jump_force = 200;
    public float climb_speed = 1;
    //
    public float sprint_walk_speed_scale;
    public float sprint_accel_scale;
    public float sprint_req_stamina_level_for_activation;//in points
    public float sprint_stamina_spend;//in points
    public float stamina_max;//point count
    public float stamina_restore_speed;//point per second
    public float health_max;//in points
    public float ray_lenght = 1;
    public float additional_lives = 1;
    // fields^

    private Rigidbody physics_component;
    private CapsuleCollider collider;
    // components^

    private bool isOnFloor = false;
    private bool isSprinting = false;
    private bool isSneaking = false;
    private bool isTimeSlowed = false;
    private bool isWalkingBanned;
    private bool isJumpButtonPressed = false;
    private bool isDownButtonPressed = false;
    // object state^

    private float current_force = 0;
    private float bouncer_loss = 1;
    private float bouncer_accel = 1;
    //
    private float stamina;
    private float health;
    private float coins;
    // variables^

    Ray ray;
    //supply variables^

    private void Awake()
    {
        physics_component = gameObject.GetComponent<Rigidbody>();
        physics_component.sleepThreshold = 0;//rigidbody never sleep now

        collider = gameObject.GetComponent<CapsuleCollider>();

        health = health_max;
        stamina = stamina_max;
    }

    private void FixedUpdate()
    {
        float MoveControl_HorizontalAxis = Input.GetAxis("Horizontal");
        bool isMoveLeft = MoveControl_HorizontalAxis < 0;
        bool isMoveRight= MoveControl_HorizontalAxis > 0;
        //
        if      (isMoveLeft ) transform.eulerAngles = v3_left_rotation;
        else if (isMoveRight) transform.eulerAngles = v3_right_rotation;
        if (isWalkingBanned == false) Walk(walk_accel * Math.Abs(MoveControl_HorizontalAxis));//front of model must be looking world-right
        //walk ability^

        isJumpButtonPressed = Input.GetAxis("Vertical") > 0;
        if (isJumpButtonPressed && isOnFloor) { Jump(jump_force); }
        //jump ability^

        bool isSprintButtonPressed = Input.GetKey(KeyCode.LeftShift);
        bool isStaminaEnough = stamina >= sprint_req_stamina_level_for_activation;
        if (isSprintButtonPressed && isStaminaEnough) {
            if (isSprinting == false)
            {
                max_walk_speed *= sprint_walk_speed_scale;
                walk_accel *= sprint_accel_scale;
            }
            stamina -= sprint_stamina_spend * Time.fixedDeltaTime;
            isSprinting = true;
        }
        else if (isSprinting == true && isOnFloor == true) {
            max_walk_speed *= 1 / sprint_walk_speed_scale;
            walk_accel *= 1 / sprint_accel_scale;
            isSprinting = false;
            //restore state after sprint ability using^
        }
        //sprint ability^

        isDownButtonPressed = Input.GetAxis("Vertical") < 0;
        if (isDownButtonPressed && isOnFloor && isSneaking == false) { collider.height /= 2; isSneaking = true; }
        else if (isSneaking && isDownButtonPressed == false) { collider.height *= 2; isSneaking = false; }
        //sneaking ability^

        bool isTimeSlowButtonPressed = Input.GetKey(KeyCode.F);
        if (isTimeSlowButtonPressed && isTimeSlowed == false)
        { Time.timeScale /= 2; isTimeSlowed = true; }
        else if (isTimeSlowed) 
        { Time.timeScale *= 2; isTimeSlowed = false; }

        if (isSprintButtonPressed == false) stamina = (stamina > stamina_max) ? stamina_max : stamina + stamina_restore_speed * Time.fixedDeltaTime;
        //restorable restoration^
    }

    private void OnCollisionStay(Collision collision)
    {
        ray.direction = Vector3.down;
        ray.origin = gameObject.transform.position;
        if (Physics.Raycast(ray, ray_lenght) && isOnFloor == false) { isOnFloor = true; GameObject.Find("Camera").GetComponent<CameraController>().Shake(0.15f, 0.1f, 0.1f); }

        if (collision.collider.gameObject.tag == "bouncer")
        {
            float max_boucer_jump_force = collision.collider.gameObject.GetComponent<BouncerSpecification>().max_boucer_jump_force;
            if (isJumpButtonPressed) current_force = current_force + bouncer_accel;
            else current_force = (current_force > bouncer_loss) ? current_force - bouncer_loss : 0;
            current_force = (current_force > max_boucer_jump_force) ? max_boucer_jump_force : current_force;
            if (current_force != 0) Jump(current_force);
            isOnFloor = false;
            GameObject.Find("Camera").GetComponent<CameraController>().Zoom(30f + (current_force / max_boucer_jump_force) * 60f);
        }
        else if (isOnFloor) current_force = 0;
        //bouncer feature^

        if (collision.collider.gameObject.tag == "climbing_wall")
        {
            if (isJumpButtonPressed) Climb(climb_speed);
            isOnFloor = false;
        }
        //climbing_wall feature^

        if (collision.collider.gameObject.tag == "fragile")
        {
            collision.collider.gameObject.GetComponent<FragilePlatform>().Tick(Time.deltaTime);
        }
        //breaking fragiles feature^
    }

    private void OnTriggerStay(Collider trigger)
    {
        if (trigger.gameObject.tag == "hp_restore")
        {
            float health_restore_count = trigger.gameObject.GetComponent<HP_Restore_item_specification>().health_restore_count;
            health = stamina = (health > health_max) ? health_max : health + health_restore_count;
            trigger.gameObject.SetActive(false);
        }

        if (trigger.gameObject.tag == "coin")
        {
            coins += 1;
            if (coins % 100 == 0) additional_lives += 1;
            trigger.gameObject.SetActive(false);
        }

        if (trigger.gameObject.tag == "note")
        {
            trigger.gameObject.GetComponent<NoteSpecification>().AddInJournal();
            trigger.gameObject.SetActive(false);
        }

        if (trigger.gameObject.tag == "bullet")
        {
            health -= trigger.gameObject.GetComponent<BulletSpecification>().damage;
            trigger.gameObject.SetActive(false);
            if (health < 1) { Death(); return; }
        }

        if (trigger.gameObject.tag == "drag_object")
        {
            gameObject.transform.position = trigger.gameObject.transform.position;

            physics_component.useGravity = false;
            physics_component.velocity = Vector3.zero;
            isWalkingBanned = true;
            isOnFloor = false;
        }

        if (trigger.gameObject.tag == "ladder")
        {
            if (isJumpButtonPressed) Climb( climb_speed);
            if (isDownButtonPressed) Climb(-climb_speed);

            physics_component.useGravity = false;
            physics_component.velocity -= physics_component.velocity.y * Vector3.up;
            isOnFloor = false;
        }
        //ladder feature^
    }
    private void OnTriggerExit(Collider trigger)
    {
        if (trigger.gameObject.tag == "ladder") physics_component.useGravity = true;
        //restore state after ladder feature using^
        if (trigger.gameObject.tag == "drag_object") { physics_component.useGravity = true; isWalkingBanned = false; }
        //restore state after crane feature using^
    }

    private void Walk(float speed)
    {
        physics_component.AddForce(gameObject.transform.right * speed, ForceMode.Acceleration);
        float direction = (physics_component.velocity.x >= 0) ? 1 :-1;
        if (Math.Abs(physics_component.velocity.x) > max_walk_speed) physics_component.velocity = physics_component.velocity + Vector3.right * (max_walk_speed*direction - physics_component.velocity.x);
    }
    private void Jump(float force)
    {
        physics_component.velocity += Vector3.up * force;
        isOnFloor = false;
    }
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
    private void Respawn() { }
}
