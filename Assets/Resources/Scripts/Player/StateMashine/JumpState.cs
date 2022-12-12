using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace stateMachinePlayer
{
    public class JumpState : State
    {
        public JumpState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {

        }        

        private float jumpVelocity = 10f;                

        public override void FixedUpdate()
        {
            if (player.OnGround)
            {
                player.Rigidbody.AddForce(Vector3.up * jumpVelocity, ForceMode.Impulse);
                stateMachine.ChangeState(new IdleState(stateMachine, player));
            }
            else 
            {
                stateMachine.ChangeState(new IdleState(stateMachine, player));
            }
        }
    }
}