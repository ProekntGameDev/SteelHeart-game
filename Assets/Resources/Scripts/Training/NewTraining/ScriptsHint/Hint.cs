using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class Hint : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string _tag = "Player"; //player tag
    [Tooltip("Don't forget to specify: <sprite=0>")]
    [SerializeField] private string _text = ""; //message text
    [SerializeField] private TMP_SpriteAsset _sprite; //sprite in the text

    private StateHint _stateHint = StateHint.Activity; //hint status

    private void OnTriggerEnter(Collider other)
    {
        if(_stateHint == StateHint.Activity) //if the hint has the state: Activity
        {
            if (other.CompareTag(_tag)) //if the object has a tag: _tag
            {
                EditorHint.OnActiveHint?.Invoke(_sprite, _text); //activating a hint
                _stateHint = StateHint.Inactivity; //change to state: Inactivity
            }
        }
    }

    private void OnEnable() { EditorHint.OnSetStateHint += SetStateHint; } //subscribing to the method: SetStateHint

    private void SetStateHint(StateHint stateHint) { _stateHint = stateHint; } //sets the state of the hint

    private void OnDisable() { EditorHint.OnSetStateHint -= SetStateHint; } //unsubscribe method: SetStateHint
}
