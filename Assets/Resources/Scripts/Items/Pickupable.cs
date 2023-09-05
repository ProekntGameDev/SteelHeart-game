using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
public abstract class Pickupable : MonoBehaviour
{
    public bool IsPickedUp { get; protected set; }

    private BoxCollider _collider;
    private Rigidbody _rb;

    private void Start()
    {
        _collider = GetComponent<BoxCollider>();
        _rb = GetComponent<Rigidbody>();
    }

    public virtual void Pickup()
    {
        if (IsPickedUp)
            return;

        IsPickedUp = true;

        _collider.enabled = false;
        _rb.isKinematic = true;
    }

    public virtual void Drop(float speed)
    {
        if (!IsPickedUp)
            return;

        IsPickedUp = false;

        _collider.enabled = true;
        _rb.isKinematic = false;
        _rb.AddForce(speed * transform.forward, ForceMode.Impulse);
    }

    public virtual void Deliver()
    {
        if (!IsPickedUp)
            return;

        Destroy(gameObject);
    }
}