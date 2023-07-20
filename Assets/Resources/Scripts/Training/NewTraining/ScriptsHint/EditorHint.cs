using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;


public class EditorHint : MonoBehaviour
{
    public static Action<TMP_SpriteAsset, string> OnActiveHint; //activates the hint
    public static Action<StateHint> OnSetStateHint; //sets the status of all prompts

    [SerializeField] private TMP_Text _textMeshPro; //link to: TMP_Text
    [SerializeField] private GameObject _panel; //link to the panel object
    [SerializeField] private float _timeActivity = 3f; //hint activity time

    private Image _image; //link to the Image component
    private const float Delta = 0.05f; //alpha channel change interval

    private Coroutine _coroutineEnableAndDisablePanel; //link to the coroutine: EnableAndDisablePanel
    private Coroutine _coroutineCountdownPanel; //link to the coroutine: Countdown

    private void Start() { _image = _panel.GetComponent<Image>(); } //getting the Image component

    private void OnEnable() { OnActiveHint += EnableHint; } //subscribing to the method: EnableHint

    private void SetTextAndSprite(TMP_SpriteAsset sprite, string text)
    {
        if(_textMeshPro != null) //if the _textMeshPro field has a reference
        {
            if(sprite != null) _textMeshPro.spriteAsset = sprite; //installing the hint sprite
            if (text != null) _textMeshPro.text = text; //setting the hint text
        }
    }

    private void EnableHint(TMP_SpriteAsset sprite, string text)
    {
        SetTextAndSprite(sprite, text); //set text and sprite hints
        if (_panel != null) _panel.SetActive(true); //enabling the hint panel
        _coroutineCountdownPanel = StartCoroutine(Countdown(_timeActivity)); //activating a hint
    }

    private void DisableHint()
    {
        if (_panel != null) _panel.SetActive(false); //turning off the hint panel
    }

    private IEnumerator Countdown(float time)
    {
        _coroutineEnableAndDisablePanel = StartCoroutine(EnableAndDisablePanel(Delta)); //appearance of a hint
        yield return new WaitForSeconds(time); //setting the time
        _coroutineEnableAndDisablePanel = StartCoroutine(EnableAndDisablePanel(-Delta)); //disappearing hint
        if (_coroutineCountdownPanel != null) StopCoroutine(_coroutineCountdownPanel);
    }

    private IEnumerator EnableAndDisablePanel(float delta)
    {
        var color1 = _image.color; //getting a color
        var color2 = _textMeshPro.color; //getting a color
        float alpha = CheckAlpha(color1.a + delta); //getting alpha
        while (alpha < 1f && alpha > 0f)
        {
            alpha = SetColor(color1, color2, CheckAlpha(alpha + delta)); //getting alpha
            yield return new WaitForEndOfFrame(); //delay
        }
        if (delta < 0f) DisableHint(); //turning off the hint panel
        if (_coroutineEnableAndDisablePanel != null) StopCoroutine(_coroutineEnableAndDisablePanel);
    }

    private float SetColor(Color color1, Color color2, float alpha)
    {
        color1.a = alpha; //installing the alpha channel
        color2.a = alpha; //installing the alpha channel
        _image.color = color1; //setting the color
        _textMeshPro.color = color2; //setting the color
        return alpha;
    }

    private void OnDisable() { OnActiveHint -= EnableHint; } //unsubscribe method: EnableHint

    private float CheckAlpha(float value)
    {
        if (value < 0f) return 0f;
        else if (value > 1f) return 1f;
        return value;
    }
}
