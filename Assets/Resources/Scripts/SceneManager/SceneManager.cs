using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;
using System.Collections;

public class SceneManager : MonoBehaviour
{
    public UnityEvent<SceneLoad> OnLoadScene;

    [Inject] private SaveManager _saveManager;
    [Inject] private SceneLoadScreen _sceneLoadScreen;

    private Coroutine _sceneLoadCoroutine;
    private List<string> _scenes = new List<string>();

    public string NextScene() => _scenes[GetNextSceneIndex()];

    public void ReloadCurrent()
    {
        StartCoroutine(LoadSceneCoroutine(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name));
    }

    public void LoadMenu()
    {
        StartCoroutine(LoadSceneCoroutine(_scenes[0]));
    }

    public AsyncOperation LoadNext(bool callback = true)
    {
        if (callback == false)
            return AsyncLoadScene(NextScene());
        else
            StartCoroutine(LoadSceneCoroutine(NextScene()));

        return null;
    }

    public void LoadFromSave()
    {
        PlayerSaveData playerSaveData = _saveManager.Load();
        StartCoroutine(LoadSceneCoroutine(playerSaveData.Scene));
    }

    public void Quit()
    {
        Application.Quit();
    }

    private AsyncOperation AsyncLoadScene(string name)
    {
        AsyncOperation asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(name);

        OnLoadScene.Invoke(new SceneLoad(asyncOperation, name));

        return asyncOperation;
    }

    private IEnumerator LoadSceneCoroutine(string name)
    {
        if (_sceneLoadCoroutine != null)
            yield break;

        yield return StartCoroutine(_sceneLoadScreen.Enable(name));

        AsyncOperation asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(name);

        _sceneLoadScreen.StartLoad(new SceneLoad(asyncOperation, name));
    }

    private void Awake()
    {
        int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;

        for (int i = 0; i < sceneCount; i++)
            _scenes.Add(Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i)));
    }

    private int GetNextSceneIndex()
    {
        int currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;

        int nextScene = 0;

        if (currentScene < _scenes.Count - 1)
            nextScene = currentScene + 1;

        return nextScene;
    }
}

public class SceneLoad
{
    public readonly string Name;
    public readonly AsyncOperation Operation;

    public SceneLoad(AsyncOperation operation, string name)
    {
        Name = name;
        Operation = operation;
    }
}
