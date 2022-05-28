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

    IEnumerator AsyncLoadScene(int id) 
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(id);
        while (op.isDone == false) 
        {
            float progress = op.progress;
            //update displaying <<<
            yield return null;
        }
    }
}
