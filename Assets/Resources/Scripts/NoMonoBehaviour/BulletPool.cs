using UnityEngine;

public class BulletPool
{
    private GameObject[] pool;
    private Bullet[] bullets;
    private GameObject bulletPrefab;


    public BulletPool(int length) 
    {
        bulletPrefab = Resources.Load<GameObject>("Prefabs/_DO_NOT_USE_MANUALLY/Bullet");
        pool = new GameObject[length];
        bullets = new Bullet[length];
        for (int i = 0; i < pool.Length; ++i)
        {
            pool[i] = MonoBehaviour.Instantiate(bulletPrefab, Vector3.zero, Quaternion.identity);
            bullets[i] = pool[i].GetComponent<Bullet>();
        }
    }

    public void Tick()
    {
        for (int i = 0; i < pool.Length; ++i)
        {
            bullets[i].Tick();
        }
    }

    public void UseBullet(Vector3 velocity, Vector3 spawnPosition)
    {
        for (int i = 0; i < bullets.Length; ++i) 
            if (pool[i].activeSelf == false)
            {
                //bullet_spec[i].damage = damage;
                bullets[i].Shoot();//_lifetime = lifetime;
                pool[i].GetComponent<Rigidbody>().velocity = velocity;
                pool[i].transform.position = spawnPosition;
                pool[i].SetActive(true);
                break;
            }
    }
}
