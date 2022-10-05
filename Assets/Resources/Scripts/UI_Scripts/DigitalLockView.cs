using TMPro;
using UnityEngine;

public class DigitalLockView : MonoBehaviour
{
    public delegate void OnNumberButtonClickDelegate(int clickedNumber);
    public delegate void OnCloseButtonClickDelegate();
    public OnNumberButtonClickDelegate OnNumberButtonClickEvent;
    public OnCloseButtonClickDelegate OnCloseButtonClickEvent;

    
    [SerializeField] private TextMeshProUGUI _outputCodeTMP;
    [SerializeField] private GameObject _digitalLockGameObject;
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

    public void OnCloseButtonClick()
    {
        _digitalLockGameObject.SetActive(false);
        OnCloseButtonClickEvent?.Invoke();
    }
}
