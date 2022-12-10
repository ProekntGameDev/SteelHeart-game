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

        public Move(StateMachine stateMachine, Player player, float speed, float rotationSpeed) : base (stateMachine, player) 
        {
            this.stateMachine = stateMachine;
            this.player = player;
            this.speed = speed;
            this.rotationSpeed = rotationSpeed;
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

            QQ();

            player.animator.SetFloat("speed", Vector3.ClampMagnitude(directionVector, 1).magnitude);
            Vector3 moveDir = Vector3.ClampMagnitude(directionVector, 1) * speed;
            player.rigidbody.velocity = new Vector3(moveDir.x, player.rigidbody.velocity.y, moveDir.z);
            player.rigidbody.angularVelocity = Vector3.zero;
        }

        protected abstract void QQ();
    }
}