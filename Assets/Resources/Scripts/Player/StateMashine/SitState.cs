using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace stateMachinePlayer
{
    public class SitState : Move
    {
        // You just need to do the animation
        public SitState(StateMachine stateMachine, Player player, float speed, float rotationSpeed) : base(stateMachine, player, speed, rotationSpeed)
        {

        }

        public override void Enter()
        {
            Debug.Log("Я в состоянии сижу!");            
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
                if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.0f || Mathf.Abs(Input.GetAxis("Horizontal")) > 0.0f)
                {
                    stateMachine.ChangeState(new RunState(stateMachine, player, 2, 10));
                }
            }            
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                stateMachine.ChangeState(new SprintState(stateMachine, player, 10, 10));
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                stateMachine.ChangeState(new JumpState(stateMachine, player));
            }           

        }

        protected override void NewAnimate()
        {
            player.animator.SetBool("isCrouching", true);
        }
    }
}