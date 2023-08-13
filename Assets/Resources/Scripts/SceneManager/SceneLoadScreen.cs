using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;
using NaughtyAttributes;

public class SceneLoadScreen : MonoBehaviour
{
    [SerializeField] private Image _loadSceneScreen;
    [SerializeField] private float _fadeTime;
    [SerializeField] private TextMeshProUGUI _sceneName;
    [SerializeField] private TextMeshProUGUI _progress;

    [Inject] private SceneManager _sceneManager;

    private void Start() 
    {
        _sceneManager.OnLoadScene.AddListener(Load);
    }

    private void Load(SceneLoad sceneLoad)
    {
        _loadSceneScreen.gameObject.SetActive(true);
        StartCoroutine(LoadingScreen(sceneLoad));
    }

    private IEnumerator LoadingScreen(SceneLoad sceneLoad)
    {
        sceneLoad.Operation.allowSceneActivation = false;

        yield return StartCoroutine(FadeIn());

        _sceneName.text = sceneLoad.Name;

        while (sceneLoad.Operation.progress < 0.9f)
        {
            _progress.text = $"{Mathf.RoundToInt(sceneLoad.Operation.progress * 100)}%";
            yield return new WaitForEndOfFrame();
        }

        sceneLoad.Operation.allowSceneActivation = true;
        _progress.text = "";
        _sceneName.text = "";

        yield return StartCoroutine(FadeOut());

        _loadSceneScreen.gameObject.SetActive(false);
    }

    private IEnumerator FadeIn()
    {
        Color color = new Color(0, 0, 0, 0);
        _loadSceneScreen.color = color;

        while (_loadSceneScreen.color.a < 1)
        {
            color.a = Mathf.Min(color.a + (1 / _fadeTime) * Time.unscaledDeltaTime, 1);
            _loadSceneScreen.color = color;
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator FadeOut()
    {
        Color color = new Color(0, 0, 0, 1);
        _loadSceneScreen.color = color;

        while (_loadSceneScreen.color.a > 0)
        {
            color.a = Mathf.Max(color.a - (1 / _fadeTime) * Time.unscaledDeltaTime, 0);
            _loadSceneScreen.color = color;
            yield return new WaitForEndOfFrame();
        }
    }
}
