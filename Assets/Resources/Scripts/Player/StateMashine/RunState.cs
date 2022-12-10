using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace stateMachinePlayer
{     


    public class RunState : State
    {
        public RunState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {

        }

        

        public float speed = 2f;
        public float rotationSpeed = 10;

        public override void Enter()
        {            
            player.animator.SetBool("isCrouching", false);
        }

        public override void Exit()
        {            
        }

        public override void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                stateMachine.ChangeState(new JumpState(stateMachine, player));
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                stateMachine.ChangeState(new SprintState(stateMachine, player));
            }
            else if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                stateMachine.ChangeState(new SitState(stateMachine, player));
            }
            else if (Input.GetAxis("Vertical") > 0.0f || Input.GetAxis("Vertical") < 0.0f)
            {
                stateMachine.ChangeState(new RunState(stateMachine, player));
            }
            else if (Input.GetAxis("Horizontal") > 0.0f || Input.GetAxis("Horizontal") < 0.0f)
            {
                stateMachine.ChangeState(new RunState(stateMachine, player));
            }
        }

        public override void FixedUpdate()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 directionVector = new Vector3(h, 0, v);
            if (directionVector.magnitude > Mathf.Abs(0.1f))
                player.transform.rotation = Quaternion.Lerp(player.transform.rotation, Quaternion.LookRotation(directionVector), Time.deltaTime * rotationSpeed);

            player.animator.SetFloat("speed", Vector3.ClampMagnitude(directionVector, 1).magnitude);
            Vector3 moveDir = Vector3.ClampMagnitude(directionVector, 1) * speed;
            player.rigidbody.velocity = new Vector3(moveDir.x, player.rigidbody.velocity.y, moveDir.z);
            player.rigidbody.angularVelocity = Vector3.zero;

        }
    }
}