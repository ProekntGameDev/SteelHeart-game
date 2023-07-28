using System;
using System.Collections.Generic;

// Note:
// Finite state machine realization from https://game.courses/bots-ai-statemachines/ (I'm too lazy to make it myself)

public class StateMachine
{
    public bool HasState => _currentState != null;

    private IState _currentState;

    private Dictionary<Type, List<Transition>> _transitions = new Dictionary<Type, List<Transition>>();
    private List<Transition> _currentTransitions = new List<Transition>();
    private List<Transition> _anyTransitions = new List<Transition>();

    private static List<Transition> EmptyTransitions = new List<Transition>(0);

    public void Tick()
    {
        var transition = GetTransition();
        if (transition != null)
            SetState(transition.To);

        _currentState?.Tick();
    }

    public void SetState(IState state)
    {
        if (state == _currentState)
            return;

        _currentState?.OnExit();
        _currentState = state;

        if (_currentState == null)
            return;

        _transitions.TryGetValue(_currentState.GetType(), out _currentTransitions);
        if (_currentTransitions == null)
            _currentTransitions = EmptyTransitions;

        _currentState.OnEnter();
    }

    public void Clear()
    {
        _currentState = null;

        _transitions = new Dictionary<Type, List<Transition>>();
        _currentTransitions = new List<Transition>();
        _anyTransitions = new List<Transition>();

        EmptyTransitions = new List<Transition>(0);
    }

    public bool IsInState(IState state) => _currentState == state;

    public Transition AddTransition(IState from, IState to, Func<bool> predicate)
    {
        if (_transitions.TryGetValue(from.GetType(), out var transitions) == false)
        {
            transitions = new List<Transition>();
            _transitions[from.GetType()] = transitions;
        }

        Transition transition = new Transition(to, predicate);
        transitions.Add(transition);
        return transition;
    }

    public Transition AddAnyTransition(IState state, Func<bool> predicate)
    {
        Transition transition = new Transition(state, predicate);
        _anyTransitions.Add(transition);
        return transition;
    }

    public class Transition
    {
        public Func<bool> Condition { get; }
        public IState To { get; }

        public Transition(IState to, Func<bool> condition)
        {
            To = to;
            Condition = condition;
        }
    }

    protected virtual Transition GetTransition()
    {
        foreach (var transition in _anyTransitions)
            if (transition.Condition())
                return transition;

        foreach (var transition in _currentTransitions)
            if (transition.Condition())
                return transition;

        return null;
    }
}