using TMPro;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class PlayerShootingAbility : MonoBehaviour
{
    [SerializeField] private float _bulletSpeed = 6f;
    [SerializeField] private int _countBullets = 100;
    [SerializeField] private float _reloadTime = 1;
    
    
    [SerializeField] private TextMeshProUGUI _countBulletsTextMesh;
    
    public bool IsShooting { get; private set; }  = false;

    private PlayerGrapplingAbility _grapplingAbility;
    private BulletPool _bulletPool;
    private float _playerHeight;
    private float _passedTime = 0;
    private bool _isCanShoot = true;

    private void Awake()
    {
        _grapplingAbility = GetComponent<PlayerGrapplingAbility>();
        _bulletPool = new BulletPool(100);
        _playerHeight = GetComponent<CapsuleCollider>().height;
        //_countBulletsTextMesh.text = _countBullets + "";
    }

    private void Update()
    {                
        // Set isCanShoot after reload timer.
        if (_isCanShoot == false && _countBullets > 0)
        {
            _passedTime += Time.deltaTime;
            if (_passedTime >= _reloadTime)
            {
                _passedTime = 0;
                _isCanShoot = true;
            }
        }
        

        if (Input.GetMouseButton(0))
        {
            if (_isCanShoot)
            {
                _countBullets--;
                if (_countBulletsTextMesh != null)                    
                        _countBulletsTextMesh.text = _countBullets + "";
                
                if (_grapplingAbility != null)
                {
                    if (_grapplingAbility.isMounting) return;
                }

                IsShooting = true;
            }
            else IsShooting = false;
            
        }
        else IsShooting = false;
    }

    private void FixedUpdate()
    {
        _bulletPool.Tick();

        if (IsShooting == false) return;

        Vector3 cursorDelta = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        Vector3 bulletSpawnpoint = transform.position + Vector3.right * Mathf.Sign(cursorDelta.x) +
                                   new Vector3(0, _playerHeight / 2);
        Vector3 direction = Vector3.Normalize(cursorDelta - bulletSpawnpoint);
        Vector3 velocity = direction * _bulletSpeed;

        _bulletPool.UseBullet(velocity, bulletSpawnpoint);
    }
    
    public void AddAmmunition(int ammunitionToAdd)
    {
        _countBullets += ammunitionToAdd;
    }
}
