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

        public override void Enter()
        {
            Debug.Log("Я в состоянии прыжка!");
        }

        public override void Exit()
        {
            Debug.Log("Я вышел из состояния прыжка!");
            //player.animator.SetBool("isInAir", false);
            //player.animator.SetBool("isJump", false);
        }

        public override void FixedUpdate()
        {
            player.rigidbody.AddForce(Vector3.up * jumpVelocity, ForceMode.Impulse);
            player.animator.SetBool("isJump", true);
            stateMachine.ChangeState(new IdleState(stateMachine, player));
        }
    }
}