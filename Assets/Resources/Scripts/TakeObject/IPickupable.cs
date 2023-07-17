using UnityEngine;

public interface IPickupable
{
    void Pickup();
    void Drop(Vector3 direction);
    void Deliver();
}