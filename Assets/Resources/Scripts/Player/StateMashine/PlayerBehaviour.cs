using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace stateMachinePlayer
{
    public class PlayerBehaviour : MonoBehaviour
    {
        [SerializeField]
        private Player player;
        public Transform target;
        private StateMachine stateMachine;



        void Start()
        {
            stateMachine = new StateMachine();
            stateMachine.Initialize(new IdleState(stateMachine, player));

        }

        void Update()
        {
            if (stateMachine.currentState != null)
                stateMachine.currentState.Update();


            //if (Input.GetKeyDown(KeyCode.Q))
            //{
            //    stateMachine.ChangeState(new JumpState(stateMachine, player));
            //}
            //else if (Input.GetKey(KeyCode.LeftShift))
            //{
            //    stateMachine.ChangeState(new SprintState(stateMachine, player));
            //}
            //else if (Input.GetKey(KeyCode.LeftControl))
            //{
            //    stateMachine.ChangeState(new SitState(stateMachine, player));
            //}
            //else if (Input.GetAxis("Vertical") > 0.0f || Input.GetAxis("Vertical") < 0.0f)
            //{
            //    stateMachine.ChangeState(new RunState(stateMachine, player));
            //}
            //else if (Input.GetAxis("Horizontal") > 0.0f || Input.GetAxis("Horizontal") < 0.0f)
            //{
            //    stateMachine.ChangeState(new RunState(stateMachine, player));
            //}


        }

        void FixedUpdate()
        {
            if (stateMachine.currentState != null)
                stateMachine.currentState.FixedUpdate();
        }
        
    }

    
}