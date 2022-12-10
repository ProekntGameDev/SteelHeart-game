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
                stateMachine.ChangeState(new SprintState(stateMachine, player, 1000, 10, 10));
            }
            else if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                stateMachine.ChangeState(new SitState(stateMachine, player, 100, 6, 1));
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                stateMachine.ChangeState(new JumpState(stateMachine, player));
            }
        }
                
    }
}