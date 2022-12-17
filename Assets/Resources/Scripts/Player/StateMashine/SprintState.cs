using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace stateMachinePlayer
{
    public class SprintState : Move
    {
        public SprintState(StateMachine stateMachine, Player player, float speed, float rotationSpeed, float limitSpeed) : base(stateMachine, player, speed, rotationSpeed, limitSpeed)
        {

        }        
        
        public override void Enter()        
        {
                        
        }

        public override void Exit()
        {
            
        }

        public override void Update()
        {
            if (Input.GetKeyUp(KeyCode.LeftShift))            
            {
                if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.0f || Mathf.Abs(Input.GetAxis("Horizontal")) > 0.0f)
                {
                    currentStateMachine.ChangeState(new RunState(currentStateMachine, currentPlayer, 200, 10, 2));
                }
            }
                       
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                currentStateMachine.ChangeState(new SitState(currentStateMachine, currentPlayer, 100, 6, 1));
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                currentStateMachine.ChangeState(new JumpState(currentStateMachine, currentPlayer));
            }
        }
        
    }
}