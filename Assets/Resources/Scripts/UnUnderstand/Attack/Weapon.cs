using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject[] weapons;


    private void Update(){   
        if(Input.GetKeyDown(KeyCode.Q)){
             for (int i=0; i < weapons.Length; i++){
                  if(weapons[i].activeInHierarchy == true)
                {
                      weapons[i].SetActive(false);
                      if(i != 0)
                      {
                           weapons[i-1].SetActive(true);
                      }
                      else 
                      {
                          weapons[weapons.Length - 1].SetActive(true);
                      }
                      break;
                  }
             }
         }  
    }
}


