using UnityEngine;

public interface IPickupable
{
    bool IsPickedUp { get; }

    Transform Pickup();
    void Drop(Vector3 direction);
    void Deliver();
}