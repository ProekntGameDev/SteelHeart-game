using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class BulletPool
{
    int pool_lenght;
    GameObject[] pool;
    BulletSpecification[] bullet_spec;
    GameObject bullet_prefab;
    public BulletPool(int pool_lenght, MonoBehaviour shooter_component) 
    {
        this.pool_lenght = pool_lenght;
        bullet_prefab = Resources.Load<GameObject>("Prefabs/_DO_NOT_USE_MANUALLY/Bullet");
        bullet_prefab.GetComponent<Bullet>().shooter_component = shooter_component;

        Init();
    }
    private void Init()
    {
        pool = new GameObject[pool_lenght];
        bullet_spec = new BulletSpecification[pool_lenght];
        for (int i = 0; i < pool.Length; ++i)
        {
            pool[i] = MonoBehaviour.Instantiate(bullet_prefab, Vector3.zero, Quaternion.identity);
            bullet_spec[i] = pool[i].GetComponent<BulletSpecification>();
        }
    }
    public void Tick()
    {
        for (int i = 0; i < pool.Length; ++i)
        {
            if (bullet_spec[i].lifetime < 0) pool[i].SetActive(false);
            else bullet_spec[i].lifetime -= Time.deltaTime;
        }
    }
    public void UseBullet(int damage, float lifetime, Vector3 velocity, Vector3 spawn_position)
    {
        for (int i = 0; i < bullet_spec.Length; ++i) 
            if (pool[i].activeSelf == false)
            {
                bullet_spec[i].damage = damage;
                bullet_spec[i].lifetime = lifetime;
                pool[i].GetComponent<Rigidbody>().velocity = velocity;
                pool[i].transform.position = spawn_position;
                pool[i].SetActive(true);
                break;
            }
    }
}
