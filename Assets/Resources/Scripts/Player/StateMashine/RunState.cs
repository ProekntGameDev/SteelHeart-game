using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace stateMachinePlayer
{     


    public class RunState : Move
    {
        public RunState(StateMachine stateMachine, Player player, float speed, float rotationSpeed, float limitSpeed) : base(stateMachine, player, speed, rotationSpeed, limitSpeed)
        {

        } 


        public override void Update()
        {            
            if (Input.GetKey(KeyCode.LeftShift))
            {
                currentStateMachine.ChangeState(new SprintState(currentStateMachine, currentPlayer, 1000, 10, 10));
            }
            else if (Input.GetKeyDown(KeyCode.LeftControl))
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