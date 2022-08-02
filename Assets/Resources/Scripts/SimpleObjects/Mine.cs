using System.Collections;
using UnityEngine;

public class Mine : MonoBehaviour, IInteractableMonoBehaviour
{
    public float timeBeforeExplosion = 2;
    public float explosionRadius = 6;
    public float damage = 100;

    public void Interact(Transform obj)
    {
        StartCoroutine(Activate());
    }

    private IEnumerator Activate()
    {
        while(timeBeforeExplosion > 0)
        {
            timeBeforeExplosion -= Time.deltaTime;
            yield return null;
        }
        Explode();
    }

    private void Explode() 
    {
        var colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var collider in colliders)
        {
            var health = collider.GetComponent<Health>();
            if (health == null) continue;

            float damageAmount = CalculateDamage(collider.transform.position);
            health.Damage(damageAmount);
        }

        gameObject.SetActive(false);
    }

    private float CalculateDamage(Vector3 position)
    {
        float distance = Vector3.Distance(position, transform.position);
        return damage / (distance * distance);
    }
}
