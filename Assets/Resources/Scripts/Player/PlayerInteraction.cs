using UnityEngine;
using Interfaces;

public class PlayerInteraction : MonoBehaviour
{
    public KeyCode interactionKey = KeyCode.E;

    private bool interactionKeyPressed;

    private void OnTriggerEnter(Collider other)
    {
        var triggerable = other.GetComponent<ITriggerableMonoBehaviour>();
        if (triggerable != null) triggerable.Trigger(transform);        
    }

    private void OnTriggerStay(Collider other)
    {
        if (interactionKeyPressed)
        {
            var interactable = other.GetComponent<IInteractableMonoBehaviour>();
            if (interactable != null) interactable.Interact(transform);
            interactionKeyPressed = false;
        }        
    }

    private void Update()
    {
        interactionKeyPressed = Input.GetKeyDown(interactionKey) || (interactionKeyPressed && Input.GetKeyUp(interactionKey) == false);
    }
}
