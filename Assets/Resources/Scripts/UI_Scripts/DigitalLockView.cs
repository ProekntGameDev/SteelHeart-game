using TMPro;
using UnityEngine;

public class DigitalLockView : MonoBehaviour
{
    public delegate void OnNumberButtonClickDelegate(int clickedNumber);
    public OnNumberButtonClickDelegate OnNumberButtonClickEvent;
    
    [SerializeField] private TextMeshProUGUI _outputCodeTMP;
    private string _outputCodeString;
    

    public void SetOutputCodeString(string newCodeString)
    {
        _outputCodeTMP.text = newCodeString;
    }

    public string GetOutputCodeString()
    {
        return _outputCodeString;
    }

    public void OnNumberButtonClick(int clickedNumber)
    {
        OnNumberButtonClickEvent?.Invoke(clickedNumber);
    }
    
}
