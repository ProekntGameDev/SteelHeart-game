using System;
using System.Collections.Generic;

public class AnimatableStateMachine : StateMachine, IAnimatableStateMachine
{
    private Dictionary<Transition, List<Action>> _transitionsAnimation = new Dictionary<Transition, List<Action>>();

    public void AddEventToTransition(Transition transition, Action action)
    {
        if (action == null)
            throw new ArgumentNullException(nameof(action));

        if (_transitionsAnimation.ContainsKey(transition) == false)
            _transitionsAnimation.Add(transition, new List<Action>() { action });
        else
            _transitionsAnimation[transition].Add(action);
    }

    protected override Transition GetTransition()
    {
        Transition transition = base.GetTransition();

        if (transition == null)
            return transition;

        if (_transitionsAnimation.ContainsKey(transition))
            foreach (var action in _transitionsAnimation[transition])
                action?.Invoke();

        return transition;
    }
}
