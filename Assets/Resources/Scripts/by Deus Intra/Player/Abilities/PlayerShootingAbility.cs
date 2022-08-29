using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class PlayerShootingAbility : MonoBehaviour
{
    [SerializeField] private float _bulletSpeed = 6f;
    
    public bool IsShooting { get; private set; }  = false;

    private PlayerGrapplingAbility _grapplingAbility;
    private BulletPool _bulletPool;
    private float _playerHeight;

    private void Awake()
    {
        _grapplingAbility = GetComponent<PlayerGrapplingAbility>();
        _bulletPool = new BulletPool(100);
        _playerHeight = GetComponent<CapsuleCollider>().height;
    }

    private void Update()
    {        
        if (Input.GetMouseButton(0))
        {
            if (_grapplingAbility != null)
            {                
                if (_grapplingAbility.isMounting) return;
            }            
            IsShooting = true;
        }
        else IsShooting = false;
    }

    private void FixedUpdate()
    {
        _bulletPool.Tick();

        if (IsShooting == false) return;

        Vector3 cursorDelta = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        Vector3 bulletSpawnpoint = transform.position + Vector3.right * Mathf.Sign(cursorDelta.x) + new Vector3(0, _playerHeight / 2);
        Vector3 direction = Vector3.Normalize(cursorDelta - bulletSpawnpoint);
        Vector3 velocity =  direction * _bulletSpeed;

        _bulletPool.UseBullet(velocity, bulletSpawnpoint);
    }
}
