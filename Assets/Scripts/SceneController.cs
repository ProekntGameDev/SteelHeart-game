using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void ReloadScene() //перезапуск сцены (после смерти)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
