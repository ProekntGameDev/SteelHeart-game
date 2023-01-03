using Common.Abstractions;
using System;
using UnityEngine;

namespace Features.Buttons
{
    public abstract class AbstractActivatedSource : MonoBehaviour, IInteractable, IActivated
    {
        public event Action Activated;
        public event Action<Action> ActivatedWithCallback;

        public void Interact()
        {
            if (TryInteract())
            {
                Activated?.Invoke();
                ActivatedWithCallback?.Invoke(CompleteHandler);
            }
        }

        protected virtual bool TryInteract() => true;
        protected virtual void CompleteHandler() { }
    }
}
