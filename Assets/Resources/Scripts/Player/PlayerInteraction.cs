using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public KeyCode interactionKey = KeyCode.E;

    private List<Collider> _collidersFromTriggers = new List<Collider>();

    
    private void Update()
    {
        if (_collidersFromTriggers.Count > 0)
        {
            if (Input.GetKeyDown(interactionKey))
            {
                foreach (var collider in _collidersFromTriggers)
                {
                    var interactable = collider.GetComponent<IInteractableMonoBehaviour>();
                    if (interactable != null) interactable.Interact(transform);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _collidersFromTriggers.Add(other);

        var triggerable = other.GetComponent<ITriggerableMonoBehaviour>();
        if (triggerable != null) triggerable.Trigger(transform);        
    }

    private void OnTriggerStay(Collider other)
    {
        // if (Input.GetKeyDown(interactionKey))
        // {
        //     var interactable = other.GetComponent<IInteractableMonoBehaviour>();
        //     if (interactable != null) interactable.Interact(transform);
        // }
    }

    private void OnTriggerExit(Collider other)
    {
        _collidersFromTriggers.Remove(other);
    }
}
