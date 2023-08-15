using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InteractableTrigger : BaseTrigger, IInteractable
{
    public new void Interact()
    {
        base.Interact();
    }
}
