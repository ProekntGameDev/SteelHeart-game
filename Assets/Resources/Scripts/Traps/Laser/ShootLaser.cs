using UnityEngine;


public class ShootLaser : MonoBehaviour
{
    [SerializeField] private Material _material;
    LaserBeam _beam;
    [SerializeField] private float _width;
    [SerializeField] private int _baseDamage;

    // Draw a ray
    private void Start()
    {
        _beam = new LaserBeam(gameObject.transform.position, gameObject.transform.right, _material, _width, _baseDamage);
    }

    private void Update()
    {
        _beam._laser.positionCount = 0;
        _beam._laserIndices.Clear();
        _beam.CastRay(transform.position, transform.right, _beam._laser);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(gameObject.transform.position, gameObject.transform.right);
    }
}
