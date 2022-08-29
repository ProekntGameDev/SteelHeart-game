using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public KeyCode interactionKey = KeyCode.E;


    private void OnTriggerEnter(Collider other)
    {
        var triggerable = other.GetComponent<ITriggerableMonoBehaviour>();
        if (triggerable != null) triggerable.Trigger(transform);
    }

    private void OnCollisionStay(Collision collision)
    {
        var interactable = collision.gameObject.GetComponent<IInteractableMonoBehaviour>();
        if (interactable != null) interactable.Interact(transform);
    }
}
