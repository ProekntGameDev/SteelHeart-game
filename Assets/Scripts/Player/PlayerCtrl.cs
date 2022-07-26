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
    public float health_max;//in points
    public float ray_lenght_default = 1;
    private float ray_lenght = 1;
    public int additional_lives = 1;
    public int jetpack_jumps;
    public int consumed_jetpack_jumps;
    // fields^

    private Rigidbody physics_component;
    private CapsuleCollider collider;
    // components^

    bool isDoubleJump_Allow = false;
    bool isNightvision_Allow = false;
    //
    private bool isOnFloor = false;
    private bool isSprinting = false;
    private bool isSneaking = false;
    private bool isTimeSlowed = false;
    private bool isBlocking = false;
    private bool isNightVisionActive = false;
    private bool isWalkingBanned = false;
    private bool isJumpButtonPressed = false;
    private bool isJumpButtonPressed_last = false;
    private bool isDownButtonPressed = false;
    [SerializeField] private Vector3 checkpoint;
    // object state^

    private float current_force = 0;
    private float bouncer_loss = 1;
    private float bouncer_accel = 1;
    //
    private float stamina;
    public float health;
    public float coins;
    // variables^

    private void Awake()
    {
        physics_component = gameObject.GetComponent<Rigidbody>();
        physics_component.sleepThreshold = 0;//rigidbody never sleep now

        collider = gameObject.GetComponent<CapsuleCollider>();

        health = health_max;
        stamina = stamina_max;

        checkpoint = gameObject.transform.position;
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
        // walk ability^

        isJumpButtonPressed = Input.GetAxis("Jump") > 0;
        bool isJustPressed = isJumpButtonPressed && isJumpButtonPressed_last == false;
        isJumpButtonPressed_last = isJumpButtonPressed;
        if (isJumpButtonPressed && isOnFloor) { Jump(jump_force); consumed_jetpack_jumps = 0; }
        else if (isDoubleJump_Allow == true && isJustPressed && consumed_jetpack_jumps < jetpack_jumps) { Jump(jump_force * 1.2f); ++consumed_jetpack_jumps; }
        // jump ability^

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
        // sprint ability^

        isDownButtonPressed = Input.GetAxis("Vertical") < 0;
        if (isDownButtonPressed && isOnFloor && isSneaking == false) { collider.height /= 2; isSneaking = true; }
        else if (isSneaking && isDownButtonPressed == false) { collider.height *= 2; isSneaking = false; }
        // sneaking ability^

        bool isTimeSlowButtonPressed = Input.GetKey(TimeSlowing_btn);
        if (isTimeSlowButtonPressed && isTimeSlowed == false)
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

        if (Input.GetMouseButton(1)) { isBlocking = stamina > block_req_stamina_level_for_activation; }
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
                    checkpoint = hit.collider.gameObject.transform.position - Vector3.forward * hit.collider.gameObject.transform.position.z;
                if (hit.collider.gameObject.tag == "restoration_point")
                    health = health_max;
            }
        }
        // clickable object using ability^

        if (health <= 0) Death();
        // death ability^

        if (isSprintButtonPressed == false) stamina = (stamina > stamina_max) ? stamina_max : stamina + stamina_restore_speed * Time.fixedDeltaTime;
        // restorable restoration^
    }

    private void OnCollisionStay(Collision collision)
    {
        RaycastHit raycast_result;
        bool isHit = Physics.Raycast(gameObject.transform.position, Vector3.down, out raycast_result, ray_lenght);
        if (isHit && isOnFloor == false) { isOnFloor = true; Camera.main.gameObject.GetComponent<CameraController>().Shake(0.15f, 0.1f, 0.1f); }

        if (collision.collider.gameObject.tag == "bouncer" && isHit && raycast_result.collider.gameObject.tag == "bouncer")
        {
            float max_boucer_jump_force = collision.collider.gameObject.GetComponent<BouncerSpecification>().max_boucer_jump_force;
            ray_lenght = ray_lenght_default*1.1f + ray_lenght_default * (current_force / max_boucer_jump_force);
            if (isJumpButtonPressed) current_force = current_force + bouncer_accel;
            else current_force = (current_force > bouncer_loss) ? current_force - bouncer_loss : 0;
            current_force = (current_force > max_boucer_jump_force) ? max_boucer_jump_force : current_force;
            if (current_force != 0) { Jump(current_force); isOnFloor = false; }
            Camera.main.gameObject.GetComponent<CameraController>().Zoom(30f + (current_force / max_boucer_jump_force) * 60f);
        }
        else if (isOnFloor) { current_force = 0; ray_lenght = ray_lenght_default; }
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
        //medkit feature^

        if (trigger.gameObject.tag == "coin")
        {
            coins += 1;
            if (coins % 100 == 0) additional_lives += 1;
            trigger.gameObject.SetActive(false);
        }
        //coin feature^

        if (trigger.gameObject.tag == "note")
        {
            trigger.gameObject.GetComponent<NoteSpecification>().AddInJournal();
            trigger.gameObject.SetActive(false);
        }
        //note feature^

        if (trigger.gameObject.tag == "mine")
        {
            trigger.gameObject.GetComponent<Mine>().Activate();
        }
        //mine feature^

        if (trigger.gameObject.tag == "bullet")
        {
            if (isBlocking) { stamina -= block_stamina_spend; return; }
            health -= trigger.gameObject.GetComponent<BulletSpecification>().damage;
            trigger.gameObject.SetActive(false);
            if (health <= 1) { Death(); return; }
        }
        //bullet feature^

        if (trigger.gameObject.tag == "drag_object")
        {
            gameObject.transform.position = trigger.gameObject.transform.position;

            physics_component.useGravity = false;
            physics_component.velocity = Vector3.zero;
            isWalkingBanned = true;
            isOnFloor = false;
        }
        //zipline/crane feature^

        if (trigger.gameObject.tag == "ladder")
        {
            if (isJumpButtonPressed) Climb( climb_speed);
            if (isDownButtonPressed) Climb(-climb_speed);

            physics_component.useGravity = false;
            physics_component.velocity -= physics_component.velocity.y * Vector3.up;
            isOnFloor = false;
            ray_lenght = 0;
        }
        else { ray_lenght = ray_lenght_default; }
        //ladder feature^

        if (trigger.gameObject.tag == "upgrade_jetpack")
        {
            jetpack_jumps = 1;
            isDoubleJump_Allow = true;
            trigger.gameObject.SetActive(false);
        }
        //jetpack feature^

        if (trigger.gameObject.tag == "upgrade_nightvision")
        {
            isNightvision_Allow = true;
            trigger.gameObject.SetActive(false);
        }
        //nightvision feature^
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
        health = 0;
        if (additional_lives > 0)
        {
            additional_lives -= 1;
            Respawn();
        }
        else Debug.Log("Death!");
    }
    private void Respawn() { gameObject.transform.position = checkpoint; health = health_max; }
}
