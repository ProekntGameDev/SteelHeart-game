using System;

namespace Common.Abstractions
{
    public interface IActivated
    {
        event Action Activated;
        event Action<Action> ActivatedWithCallback;
    }
}
