using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP_Bar : MonoBehaviour
{
    public Image image;
    PlayerCtrl player_ctrl;
    private  Gradient _gradient;
     
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
