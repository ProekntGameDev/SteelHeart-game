using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP_Bar : MonoBehaviour
{
    UnityEngine.UI.Image image;
    PlayerCtrl player_ctrl;
    private void Start()
    {
        image = gameObject.GetComponent<UnityEngine.UI.Image>();
        player_ctrl = FindObjectOfType<PlayerCtrl>();
    }
    void Update()
    {
        float hp_percent = player_ctrl.health / player_ctrl.health_max;
        float a = hp_percent * 19f;
        int h = Mathf.FloorToInt(a+1);
        image.fillAmount = h/19f;
    }
}
