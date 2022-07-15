using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinTrigger : MonoBehaviour
{

       
       private void OnTriggerEnter(Collider coll)
    
       {
            if(coll.CompareTag("Player"))
            {
                Coin.coin++;
                Destroy(gameObject);
            }
       }


}

