using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
public class PickupableItem : MonoBehaviour, IPickupable
{
    public bool IsPickedUp { get; private set; }

    private BoxCollider _collider;
    private Rigidbody _rb;

    private void Start()
    {
        _collider = GetComponent<BoxCollider>();
        _rb = GetComponent<Rigidbody>();
    }

    public Transform Pickup()
    {
        if (IsPickedUp) 
            return null;

        IsPickedUp = true;
        // Turn off collider and physics
        _collider.isTrigger = true;
        _rb.isKinematic = true;

        return transform;
    }

    public void Drop(Vector3 force)
    {
        if (!IsPickedUp) 
            return;

        IsPickedUp = false;

        _collider.isTrigger = false;
        _rb.isKinematic = false;
        
        _rb.AddForce(force, ForceMode.Impulse);
    }
    // Deliver method realization from interface IPickupable
    public void Deliver()
    {
        if (!IsPickedUp) 
            return;

        Destroy(gameObject);
    }
}
