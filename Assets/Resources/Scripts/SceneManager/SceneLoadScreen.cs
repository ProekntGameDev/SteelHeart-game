using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SceneLoadScreen : MonoBehaviour
{
    [SerializeField] private Image _loadSceneScreen;
    [SerializeField] private float _fadeTime;
    [SerializeField] private TextMeshProUGUI _sceneName;
    [SerializeField] private TextMeshProUGUI _progress;

    private void Awake()
    {
        SetAlpha(0);
    }

    public IEnumerator Enable(string sceneName)
    {
        _sceneName.text = sceneName;
        _progress.text = "0%";

        yield return StartCoroutine(FadeIn());
    }

    public void StartLoad(SceneLoad sceneLoad)
    {
        StartCoroutine(LoadingScreen(sceneLoad));
    }

    private IEnumerator LoadingScreen(SceneLoad sceneLoad)
    {
        sceneLoad.Operation.allowSceneActivation = false;

        while (sceneLoad.Operation.progress < 0.9f)
        {
            _progress.text = $"{Mathf.RoundToInt(sceneLoad.Operation.progress * 100)}%";
            yield return new WaitForEndOfFrame();
        }

        _progress.text = "100%";
        sceneLoad.Operation.allowSceneActivation = true;

        yield return new WaitForSceneLoad(sceneLoad.Operation);

        yield return StartCoroutine(FadeOut());
    }

    private IEnumerator FadeIn()
    {
        SetAlpha(0);
        CrossFadeAlpha(1);
        yield return new WaitForSeconds(_fadeTime);
    }

    private IEnumerator FadeOut()
    {
        SetAlpha(1);
        CrossFadeAlpha(0);
        yield return new WaitForSeconds(_fadeTime);
    }

    private void CrossFadeAlpha(float value)
    {
        foreach (Graphic graphic in GetGraphics())
            graphic.CrossFadeAlpha(value, _fadeTime, false);
    }

    private void SetAlpha(float value)
    {
        foreach (Graphic graphic in GetGraphics())
            graphic.CrossFadeAlpha(value, 0, true);
    }

    private IEnumerable<Graphic> GetGraphics()
    {
        List<Graphic> result = new List<Graphic>();

        _loadSceneScreen.GetComponentsInChildren(result);

        return result;
    }
}
