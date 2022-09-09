using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    public int damage;
    public float boom_range;

    private SphereCollider _collider;
    public List<EnemyController> enemy_storage = new List<EnemyController>();

    private void Start()
    {
        _collider = gameObject.GetComponent<SphereCollider>();
        _collider.radius = boom_range;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "bullet") 
        {
            bool inRange = Vector3.Distance(FindObjectOfType<PlayerMovement>().transform.position, transform.position) < boom_range;
            //if (inRange) FindObjectOfType<PlayerController>().health -= damage;

            foreach (EnemyController enemy in enemy_storage) enemy.health -= damage;

            gameObject.SetActive(false);
        }
    }

    public void DamageSubscribe(EnemyController sender)
    {
        enemy_storage.Add(sender);
    }
    public void CancelDamageSubscription(EnemyController sender)
    {
        if (enemy_storage.Contains(sender))
            enemy_storage.Remove(sender);
    }
}
