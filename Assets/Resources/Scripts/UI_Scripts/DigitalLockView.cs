using Interfaces;
using TMPro;
using UnityEngine;

public class DigitalLockView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _outputCodeTMP;
    [SerializeField] private MonoBehaviour _controller;
    private INumberButtonClickable _numberButtonClickable;
    private string _outputCodeString;


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

    public void OnClickNumberButton(int clickedNumber)
    {
        _numberButtonClickable.OnClickNumberButton(clickedNumber);
    }
}