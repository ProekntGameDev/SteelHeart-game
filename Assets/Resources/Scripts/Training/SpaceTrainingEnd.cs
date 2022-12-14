using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceTrainingEnd : MonoBehaviour
{
    [SerializeField] private GameObject _buttonSpace;

    private void OnTriggerEnter(Collider other)
    {        
        if (other.CompareTag("Player"))
        {            
            Destroy(_buttonSpace);           
        }
    }
}
