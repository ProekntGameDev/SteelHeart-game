using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingWASD : MonoBehaviour
{
    [SerializeField] private GameObject _buttonWASD;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _buttonWASD.SetActive(true);
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(_buttonWASD);        }
    }

    
}
