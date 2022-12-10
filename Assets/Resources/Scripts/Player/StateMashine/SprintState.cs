using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace stateMachinePlayer
{
    public class SprintState : State
    {
        public SprintState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {

        }        


        public float speed = 4f;
        public float rotationSpeed = 10;


        public override void Enter()        
        {
                        
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
            else if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                stateMachine.ChangeState(new SitState(stateMachine, player));
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