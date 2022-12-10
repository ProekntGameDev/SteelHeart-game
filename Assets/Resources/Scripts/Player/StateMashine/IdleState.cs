using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace stateMachinePlayer
{   

    public class IdleState : State
    {
        public IdleState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {

        }                

        public override void Enter()
        {
            Debug.Log("Я в состоянии покоя!");            
        }

        public override void Exit()
        {
            Debug.Log("Я вышел из состояния покоя!");
        }

        public override void Update()
        {
            if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.0f || Mathf.Abs(Input.GetAxis("Horizontal")) > 0.0f)
            {
                stateMachine.ChangeState(new RunState(stateMachine, player, 2, 10));
            }
            else if (Input.GetKey(KeyCode.LeftShift))
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