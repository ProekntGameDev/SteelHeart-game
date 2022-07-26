using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public float time;
    public float boom_range;
    bool isActivated = false;
    PlayerCtrl player_ctrl;
    public void Activate()
    {
        isActivated = true;
        player_ctrl = FindObjectOfType<PlayerCtrl>();
    }
    void Update()
    {
        if (isActivated == false) return;
        if (time < 0)
        {
            if (Vector3.Distance(player_ctrl.gameObject.transform.position, gameObject.transform.position) < boom_range) player_ctrl.Death();
            gameObject.SetActive(false);
        }
        else time -= Time.deltaTime;
    }
}
