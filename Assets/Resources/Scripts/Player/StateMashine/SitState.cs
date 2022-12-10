using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace stateMachinePlayer
{
    public class SitState : State
    {
        // You just need to do the animation
        public SitState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {

        }

        public float speed = 1f;
        public float rotationSpeed = 6;
        public bool sit = true;

        public override void Enter()
        {
            Debug.Log("Я в состоянии сижу!");
            sit = true;
        }

        public override void Exit()
        {
            Debug.Log("Я вышел из состояния сижу!");
            player.animator.SetBool("isCrouching", false);
        }
        public override void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                sit = false;
                stateMachine.ChangeState(new IdleState(stateMachine, player));
            }

            if (!sit)
            {               
                if (Input.GetAxis("Vertical") > 0.0f || Input.GetAxis("Vertical") < 0.0f)
                {
                    stateMachine.ChangeState(new RunState(stateMachine, player));
                }
                else if (Input.GetAxis("Horizontal") > 0.0f || Input.GetAxis("Horizontal") < 0.0f)
                {
                    stateMachine.ChangeState(new RunState(stateMachine, player));
                }                
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                stateMachine.ChangeState(new JumpState(stateMachine, player));
            } 
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                stateMachine.ChangeState(new SprintState(stateMachine, player));
            }

        }

        public override void FixedUpdate()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 directionVector = new Vector3(h, 0, v);
            if (directionVector.magnitude > Mathf.Abs(0.1f))
                player.transform.rotation = Quaternion.Lerp(player.transform.rotation, Quaternion.LookRotation(directionVector), Time.deltaTime * rotationSpeed);

            player.animator.SetBool("isCrouching", true);
            //player.animator.SetFloat("speed", Vector3.ClampMagnitude(directionVector, 1).magnitude);
            Vector3 moveDir = Vector3.ClampMagnitude(directionVector, 1) * speed;
            player.rigidbody.velocity = new Vector3(moveDir.x, player.rigidbody.velocity.y, moveDir.z);
            player.rigidbody.angularVelocity = Vector3.zero;

        }
    }
}