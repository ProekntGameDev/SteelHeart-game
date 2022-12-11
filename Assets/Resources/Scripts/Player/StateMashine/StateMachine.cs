using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace stateMachinePlayer
{
    public class StateMachine
    {
        public State currentState { get; set; }

        public void Initialize(State startState)
        {
            currentState = startState;
            currentState.Enter();
        }

        public void ChangeState(State newState)
        {
            currentState.Exit();
            currentState = newState;
            currentState.Enter();
        }
    }
}