using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    [SerializeField] private Transform holdingPosition;   
    private PickupObject _pickupObject;
    private Vector3 _additionalVector;

    private void OnTriggerStay(Collider other)
    {
        // Get scripts
        IPickupable pickupable = other.GetComponent<IPickupable>();
        ReceivingObject receiver = other.GetComponent<ReceivingObject>();
        
        if (pickupable == null && receiver == null) return;
        // Check key
        if (Input.GetKeyDown(KeyCode.E))
        {
            // if try to take object
            if (receiver == null && pickupable != null)
            {
                _pickupObject = other.GetComponent<PickupObject>();
                _pickupObject.Pickup();
                return;
            }

            // if try to give object to receiver
            if (receiver != null && !receiver.Received && _pickupObject != null)
            {
                _pickupObject.Deliver();
                receiver.ReceivedItem();
                Debug.Log("Delivered");
            }
        }
    }

    void Update()
    {
        // If object is taken, it moves with the player
        if (_pickupObject != null && _pickupObject.isPickedUp)
        {
            _additionalVector = transform.right * 0.25f + transform.up * (holdingPosition.localPosition.y - 0.07f) + transform.forward * 0.06f;
            
            // Movement of object
            _pickupObject.transform.position = transform.position + _additionalVector;
            // Rotation of the object
            _pickupObject.transform.eulerAngles =  new Vector3(holdingPosition.eulerAngles.x, 0, transform.eulerAngles.y - 115);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            if (_pickupObject != null && _pickupObject.isPickedUp)
            {
                _pickupObject.Drop(transform.up * 3f + transform.forward);
                // Reset reference
                _pickupObject = null;
            }
        }
    }
}
