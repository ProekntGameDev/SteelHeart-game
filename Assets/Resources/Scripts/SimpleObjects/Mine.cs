using System.Collections;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public float damage = 100f;
    public float timeToExplode = 2f;
    public float radius = 1.5f;


    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(activationCoroutine());
    }

    private IEnumerator activationCoroutine()
    {
        float time = timeToExplode;
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (var collider in colliders)
        {
            var health = collider.GetComponent<Health>();
            if (health != null) health.Damage(damage);
        }
        gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
