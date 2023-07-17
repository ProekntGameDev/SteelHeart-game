using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour, IPickupable
{
    public bool isPickedUp;
    private BoxCollider _collider;
    private Rigidbody _rb;

    private void Start()
    {
        _collider = GetComponent<BoxCollider>();
        _rb = GetComponent<Rigidbody>();
    }

    // Pickup method realization from interface IPickupable
    public void Pickup()
    {
        // Check
        if (isPickedUp) return;
        
        isPickedUp = true;
        // Turn off collider and physics
        _collider.isTrigger = true;
        _rb.isKinematic = true;
    }

    // Drop method realization from interface IPickupable
    public void Drop(Vector3 direction)
    {
        // Check
        if (!isPickedUp) return;
        
        // Object released
        isPickedUp = false;
        // Turn on collider and physics
        _collider.isTrigger = false;
        _rb.isKinematic = false;
        
        _rb.AddForce(direction * 3f, ForceMode.Impulse);
    }
    // Deliver method realization from interface IPickupable
    public void Deliver()
    {
        if (!isPickedUp) return;
        // Card disappears
        Destroy(gameObject);
    }
}
