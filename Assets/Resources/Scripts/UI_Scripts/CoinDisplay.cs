using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinDisplay : MonoBehaviour
{
    Text text;
    PlayerController player_ctrl;
    private void Start()
    {
        text = gameObject.GetComponent<UnityEngine.UI.Text>();
        player_ctrl = FindObjectOfType<PlayerController>();
    }
    void Update()
    {
        text.text = player_ctrl.coins.ToString();
    }
}
