
using Interfaces;
using TMPro;
using UnityEngine;

public class DigitalLockView : MonoBehaviour
{

    public delegate void OnNumberButtonClickDelegate(int clickedNumber);
    public delegate void OnCloseButtonClickDelegate();
    public OnNumberButtonClickDelegate OnNumberButtonClickEvent;
    public OnCloseButtonClickDelegate OnCloseButtonClickEvent;
    
    [SerializeField] private TextMeshProUGUI _outputCodeTMP;
    [SerializeField] private MonoBehaviour _controller;
    [SerializeField] private GameObject _digitalLockGameObject;
    
    private string _outputCodeString;
    private INumberButtonClickable _numberButtonClickable;

    private void Awake()
    {
        _numberButtonClickable = _controller.GetComponent<INumberButtonClickable>();
    }

    public void SetCode(int newCode)
    {
        _outputCodeTMP.text = newCode.ToString();
    }
    public void SetOutputCodeString(string newCodeString)
    {
        _outputCodeTMP.text = newCodeString;
    }

    public string GetOutputCode()
    {
        return _outputCodeString;
    }

    public void OnNumberButtonClick(int clickedNumber)
    {
        OnNumberButtonClickEvent?.Invoke(clickedNumber);
    }

    public void OnCloseButtonClick()
    {
        _digitalLockGameObject.SetActive(false);
        OnCloseButtonClickEvent?.Invoke();
    }
}
