using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public float time;
    float counter;
    void Activate()
    {
        counter = time;
    }
    void Update()
    {
        if (counter < 0)
        {
            FindObjectOfType<PlayerCtrl>().Death();
            gameObject.SetActive(false);
        }
        else counter -= Time.deltaTime;
    }
}
