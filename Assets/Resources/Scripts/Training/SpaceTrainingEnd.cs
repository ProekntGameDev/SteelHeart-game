using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceTrainingEnd : MonoBehaviour
{
    [SerializeField] private GameObject _buttonSpace;
    [SerializeField] private GameObject _trigger;

    private void OnTriggerEnter(Collider other)
    {        
        if (other.CompareTag("Player"))
        {            
            Destroy(_buttonSpace);
            Destroy(_trigger);
            Destroy(this.gameObject);
        }
    }
}
