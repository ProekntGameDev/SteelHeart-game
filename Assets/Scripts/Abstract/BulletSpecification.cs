using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpecification : MonoBehaviour
{
    public int damage;
    private float lifetime;
    private Rigidbody physics_component;

    private void Start()
    {
        physics_component = gameObject.GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (lifetime > 0) { lifetime -= Time.deltaTime; }
        else gameObject.SetActive(false);
    }
    public void Activate(int damage, float lifetime, Vector3 velocity, Vector3 spawn_position) 
    {
        this.damage = damage;
        this.lifetime = lifetime;
        physics_component.velocity = velocity;
        gameObject.transform.position = spawn_position;
        gameObject.SetActive(true);
    }
}
