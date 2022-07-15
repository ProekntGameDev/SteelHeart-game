using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coin : MonoBehaviour
{
          public Text coinText;
          public static int coin;
    

    
    void Update()
    {
        coinText.text = "" +coin;
    }
}
