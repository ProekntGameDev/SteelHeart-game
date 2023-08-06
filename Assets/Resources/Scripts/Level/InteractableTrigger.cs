using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InteractableTrigger : Trigger, IInteractable
{
    public new void Interact()
    {
        base.Interact();
    }

    protected override void OnTriggerEnter(Collider other)
    {
    }
}
