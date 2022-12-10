using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace stateMachinePlayer
{     


    public class RunState : Move
    {
        public RunState(StateMachine stateMachine, Player player, float speed, float rotationSpeed) : base(stateMachine, player, speed, rotationSpeed)
        {

        } 


        public override void Update()
        {            
            if (Input.GetKey(KeyCode.LeftShift))
            {
                stateMachine.ChangeState(new SprintState(stateMachine, player, 10, 10));
            }
            else if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                stateMachine.ChangeState(new SitState(stateMachine, player, 1, 6));
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                stateMachine.ChangeState(new JumpState(stateMachine, player));
            }
        }
                
    }
}