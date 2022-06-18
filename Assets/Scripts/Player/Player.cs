using System;
using UnityEngine;

/*
WARNING!!! ALARM!!!
front of model must be looking world-right!
WARNING!!! ALARM!!!
*/
public class Player : MonoBehaviour
{
    private Vector3 v3_left_rotation = new Vector3(0, 180, 0);
    private Vector3 v3_right_rotation = new Vector3(0, 0, 0);//front of model must be looking world-right
    //euler-angle values vectors^ constants^

    public float walk_accel = 200;
    public float max_walk_speed = 1;
    public float jump_force = 200;
    public float max_boucer_jump_force = 400;
    public float climb_speed = 1;
    // fields^

    private Rigidbody physics_component;
    // components^

    private bool isOnFloor = false;
    private bool isJumpButtonPressed = false;
    private bool isDownButtonPressed = false;
    // object state^

    private float current_force = 0;
    private float bouncer_loss = 40;
    private float bouncer_accel = 40;
    // variables^

    private void Awake()
    {
        physics_component = gameObject.GetComponent<Rigidbody>();
        physics_component.sleepThreshold = 0;//rigidbody never sleep now
    }

    private void FixedUpdate()
    {
        float MoveControl_HorizontalAxis = Input.GetAxis("Horizontal");
        bool isMoveLeft = MoveControl_HorizontalAxis < 0;
        bool isMoveRight= MoveControl_HorizontalAxis > 0;
        //
        if      (isMoveLeft ) transform.eulerAngles = v3_left_rotation;
        else if (isMoveRight) transform.eulerAngles = v3_right_rotation;
        Walk(walk_accel * Math.Abs(MoveControl_HorizontalAxis));//front of model must be looking world-right
        //movement ability^

        isJumpButtonPressed = Input.GetAxis("Vertical") > 0;
        if (isJumpButtonPressed && isOnFloor) Jump(jump_force);
        //jump ability^

        isDownButtonPressed = Input.GetAxis("Vertical") < 0;
        //supply data update^
    }

    private void OnCollisionStay(Collision collision)
    {
        isOnFloor = true;

        if (collision.collider.gameObject.tag == "bouncer")
        {
            if (isJumpButtonPressed) current_force = current_force + bouncer_accel;
            else current_force = (current_force > bouncer_loss) ? current_force - bouncer_loss : 0;
            current_force = (current_force > max_boucer_jump_force) ? max_boucer_jump_force : current_force;
            if (current_force != 0) Jump(current_force);
            isOnFloor = false;
        }
        //bouncer feature^

        if (collision.collider.gameObject.tag == "climbing_wall")
        {
            if (isJumpButtonPressed) Climb(climb_speed);
            isOnFloor = false;
        }
        //climbing_wall feature^
    }

    private void OnTriggerStay(Collider trigger)
    {
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
    }

    private void Walk(float speed)
    {
        physics_component.AddForce(gameObject.transform.right * speed, ForceMode.Acceleration);
        float direction = (physics_component.velocity.x >= 0) ? 1 :-1;
        if (Math.Abs(physics_component.velocity.x) > max_walk_speed) physics_component.velocity = physics_component.velocity + Vector3.right * (max_walk_speed*direction - physics_component.velocity.x);
    }
    private void Jump(float force)
    {
        physics_component.AddForce(Vector3.up * force, ForceMode.Force);
        isOnFloor = false;
    }
    private void Climb(float speed) 
    {
        gameObject.transform.position += gameObject.transform.up * speed * Time.fixedDeltaTime;
    }
}
