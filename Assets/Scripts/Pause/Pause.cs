using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public bool PauseGame;
    public GameObject pauseGameMenu;

    // Update is called once per frame
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(PauseGame)
            {
                Resume();
            }
            else 
            {
                PauseMenu();
            }
        }   
    }


         public void Resume() {
           pauseGameMenu.SetActive(false);
           Time.timeScale = 1f;
           PauseGame = false;
         }
          public void PauseMenu() {
           pauseGameMenu.SetActive(true);
           Time.timeScale = 0f;
           PauseGame = true;
       }
         
           public void LoadMenu() {
           Time.timeScale = 1f;
           SceneManager.LoadScene("Menu");
       }
    
}