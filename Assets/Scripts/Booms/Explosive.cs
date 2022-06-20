using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    public int damage;
    public float boom_range;

    private SphereCollider collider;
    public List<Enemy> enemy_storage = new List<Enemy>();

    private void Start()
    {
        collider = gameObject.GetComponent<SphereCollider>();
        collider.radius = boom_range;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "bullet") 
        {
            bool inRange = Vector3.Distance(FindObjectOfType<PlayerCtrl>().gameObject.transform.position, gameObject.transform.position) < boom_range;
            if (inRange) FindObjectOfType<PlayerCtrl>().health -= damage;

            foreach (Enemy enemy in enemy_storage) enemy.health -= damage;

            gameObject.SetActive(false);
        }
    }

    public void DamageSubscribe(Enemy sender)
    {
        enemy_storage.Add(sender);
    }
    public void CancelDamageSubscription(Enemy sender)
    {
        if (enemy_storage.Contains(sender))
            enemy_storage.Remove(sender);
    }
}
