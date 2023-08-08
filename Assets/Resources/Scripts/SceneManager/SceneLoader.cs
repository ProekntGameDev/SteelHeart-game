using System.Net.NetworkInformation;
using System.ComponentModel;
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
    [SerializeField] private GameObject _menu;

    private void Start() 
    {
        
        _sceneManager.OnLoadScene.AddListener(Load);
    }

    private void Load(SceneLoadOperation operation)
    {
        _loadSceneScreen.SetActive(true);
        
        if(_menu!=null)_menu.SetActive(false);

        StartCoroutine(AsyncLoad(operation));
    }

    private IEnumerator AsyncLoad(SceneLoadOperation operation)
    {
        operation.Operation.allowSceneActivation = false;        
        
        _sceneName.text = operation.Name;

        while (operation.Operation.progress!=.9f)
        {
            _progress.text =operation.Operation.progress.ToString();
            yield return new WaitForEndOfFrame();

        }

        operation.Operation.allowSceneActivation = true;
        
        _loadSceneScreen.SetActive(false);
        
        _menu.SetActive(true);
        
        yield return null;
    }
}