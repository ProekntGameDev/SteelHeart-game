using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace stateMachinePlayer
{
    public abstract class Move : State
    {
        protected Player player;
        protected StateMachine stateMachine;
        protected float speed;
        protected float rotationSpeed;
        protected float limitSpeed;

        public Move(StateMachine stateMachine, Player player, float speed, float rotationSpeed, float limitSpeed) : base (stateMachine, player) 
        {
            this.stateMachine = stateMachine;
            this.player = player;
            this.speed = speed;
            this.rotationSpeed = rotationSpeed;
            this.limitSpeed = limitSpeed;
        }

        public override void Enter()
        {
        }

        public override void Exit()
        {
        }

        public override void Update()
        {
        }

        public override sealed void FixedUpdate()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 directionVector = new Vector3(h, 0, v);
            if (directionVector.magnitude > Mathf.Abs(0.1f))
                player.transform.rotation = Quaternion.Lerp(player.transform.rotation, Quaternion.LookRotation(directionVector), Time.deltaTime * rotationSpeed);

            NewAnimate();

            player.Animator.SetFloat("speed", Vector3.ClampMagnitude(directionVector, 1).magnitude);           

            Vector3 moveDir = directionVector * speed;
            moveDir.y = 0;


            var velocity = player.Rigidbody.velocity;
            if (velocity.x + velocity.z < limitSpeed)
            {
                player.Rigidbody.AddForce(moveDir);
            }                       
        }

        protected virtual void NewAnimate()
        { 
        }
    }
}