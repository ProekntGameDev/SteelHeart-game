using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragilePlatform : MonoBehaviour
{
    public float time = 2;
    public void Tick(float time = 0)
    {
        if (this.time < 0)
        {
            gameObject.SetActive(false);
        }
        else this.time -= time;
    }
}
