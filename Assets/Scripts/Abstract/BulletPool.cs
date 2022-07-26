using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    [SerializeField]int pool_lenght;
    GameObject[] pool;
    BulletSpecification[] bullet_spec;
    [SerializeField]GameObject bullet_prefab;
    int pointer;
    private void Fill()
    {
        pool = new GameObject[pool_lenght];
        bullet_spec = new BulletSpecification[pool_lenght];
        for (int i = 0; i < pool.Length; ++i)
        {
            pool[i] = Instantiate(bullet_prefab, Vector3.zero, Quaternion.identity);
            bullet_spec[i] = pool[i].GetComponent<BulletSpecification>();
        }
    }
    public void UseBullet(int damage, float lifetime, Vector3 velocity, Vector3 spawn_position) 
    {
        if (pool == null) Fill();
        for (int i = 0; i < bullet_spec.Length; ++i) { if (pool[i].activeSelf == false) { bullet_spec[i].Activate(damage, lifetime, velocity, spawn_position); return; } }
    }
}
