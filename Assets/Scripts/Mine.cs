using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public float time;
    bool isActivated = false;
    void Activate()
    {
        isActivated = true;
    }
    void Update()
    {
        if (isActivated == false) return;
        if (time < 0)
        {
            FindObjectOfType<PlayerCtrl>().Death();
            gameObject.SetActive(false);
        }
        else time -= Time.deltaTime;
    }
}
