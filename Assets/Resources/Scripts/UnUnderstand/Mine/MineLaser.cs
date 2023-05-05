using UnityEngine;


public class MineLaser : MonoBehaviour
{
    //public GameObject explosionPrefab;
    [SerializeField] private float explosionRadius = 5f;

    private LineRenderer line;
    private RaycastHit hit;

    public void Start(){
        line = GetComponent<LineRenderer>();
    }

    public void Update()
    {
        Vector3 startPoint = transform.position;
        Vector3 endPoint = transform.forward * 50f;

        line.SetPosition(0, startPoint);
        line.SetPosition(1, endPoint);

        if (Physics.Linecast(startPoint, endPoint, out hit))
            Explode();
    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rigidbody = nearbyObject.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.AddExplosionForce(100f, transform.position, explosionRadius);
                if (hit.collider.TryGetComponent(out Player player))
                {
                    player.Health.TakeDamage(player.Health.Max);
                }
            }
        }

        Destroy(gameObject);
    }
}