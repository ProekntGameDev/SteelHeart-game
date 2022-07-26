using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadScreen : MonoBehaviour
{
    public void Restart(){
         SceneManager.LoadScene("ForTests1");
    }


     public void LoadGame(){

         SceneManager.LoadScene("Menu");
     }
}
