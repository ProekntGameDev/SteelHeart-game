using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private GameObject _loadSceneScreen;
    [SerializeField] private TextMeshProUGUI _sceneName;
    [SerializeField] private TextMeshProUGUI _progress;
    [SerializeField] private SceneManager _sceneManager;

    private void Start() 
    {
        _sceneManager.OnLoadScene.AddListener(Load);
    }

    private void Load(SceneLoadOperation operation)
    {
        _loadSceneScreen.SetActive(true);

        StartCoroutine(AsyncLoad(operation));
    }

    private IEnumerator AsyncLoad(SceneLoadOperation operation)
    {
        operation.Operation.allowSceneActivation = false;

        _sceneName.text = operation.Name;

        while (!operation.Operation.isDone)
        {
            _progress.text =operation.Operation.progress.ToString();
            yield return new WaitForEndOfFrame();
        }

        operation.Operation.allowSceneActivation = true;

        _loadSceneScreen.SetActive(false);
    }

}
