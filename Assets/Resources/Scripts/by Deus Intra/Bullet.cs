using System;
using UnityEngine;

public class Bullet : MonoBehaviour, IInteractableMonoBehaviour
{
    public float damage;
    public MonoBehaviour shooter_component;

    public void Interact(Transform obj)
    {
        // Don't take damage from player bullet.
        if (obj.GetComponent<PlayerController>() != null)
        {
            if (obj.GetComponent<PlayerController>().IsBulletDefenderActive)
            {
                Vector3 current_velocity = gameObject.GetComponent<Rigidbody>().velocity;
                Translate(current_velocity * -1);
                return;
            }
            
            if (shooter_component == obj.GetComponent<PlayerController>())
            {
                return;
            }
        }
        
        var health = obj.GetComponent<Health>();
        if (health == null) return;

        health.Damage(damage);
        gameObject.SetActive(false);
    }

    public void Translate(Vector3 velocity)
    {
        gameObject.GetComponent<Rigidbody>().velocity = velocity;
    }
    
}
