using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace stateMachinePlayer
{
    public class SitState : Move
    {
        // You just need to do the animation
        public SitState(StateMachine stateMachine, Player player, float speed, float rotationSpeed, float limitSpeed) : base(stateMachine, player, speed, rotationSpeed, limitSpeed)
        {

        }

        public override void Enter()
        {
            Debug.Log("Я в состоянии сижу!");            
        }

        public override void Exit()
        {
            Debug.Log("Я вышел из состояния сижу!");
            currentPlayer.Animator.SetBool("isCrouching", false);
        }
        public override void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.0f || Mathf.Abs(Input.GetAxis("Horizontal")) > 0.0f)
                {
                    currentStateMachine.ChangeState(new RunState(currentStateMachine, currentPlayer, 200, 10, 2));
                }
            }            
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                currentStateMachine.ChangeState(new SprintState(currentStateMachine, currentPlayer, 1000, 10, 10));
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                currentStateMachine.ChangeState(new JumpState(currentStateMachine, currentPlayer));
            }           

        }

        protected override void NewAnimate()
        {
            currentPlayer.Animator.SetBool("isCrouching", true);
        }
    }
}