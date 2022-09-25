using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class DigitalLock : MonoBehaviour
{
    [SerializeField] private int _correctCode = 1111;
    [Header("Relations")]
    [SerializeField] private UnityEvent _onCorrectCodeInputted;
    [SerializeField] private UnityEvent _onWrongCodeInputted;
    [SerializeField] private DigitalLockView _digitalLockView;
    
    private string _stringUserCode;
    private bool _isWorking = true;


    private void OnEnable()
    {
        _digitalLockView.OnNumberButtonClickEvent += OnNumberButtonClick;
    }
    
    private void OnDisable()
    {
        _digitalLockView.OnNumberButtonClickEvent -= OnNumberButtonClick;
    }

    
    public void SetCorrectCode(int newCode)
    {
        _correctCode = newCode;
    }

    public void OnWrongCodeInputted()
    {
        _stringUserCode = "";
        _digitalLockView.SetOutputCodeString(_stringUserCode);
    }

    public void OnCorrectCodeInputted()
    {
        _isWorking = false;
    }
    
    
    private void OnNumberButtonClick(int clickedNumber)
    {
        if (_isWorking == false) return;
        
        _stringUserCode += clickedNumber;
        int userCode = Int32.Parse(_stringUserCode);
        _digitalLockView.SetOutputCodeString(_stringUserCode);
        
        if (_stringUserCode.Length == _correctCode.ToString().Length)
        {
            if (userCode == _correctCode)
            {
                // On code is correct.
                _onCorrectCodeInputted?.Invoke();
            }
            else
            {
                _onWrongCodeInputted?.Invoke();
            }
        }
    }
}
