using System;

public interface IAnimatableStateMachine
{
    public void AddEventToTransition(StateMachine.Transition transition, Action action);
}
