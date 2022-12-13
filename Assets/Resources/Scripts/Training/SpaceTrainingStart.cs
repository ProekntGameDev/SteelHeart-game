using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpaceTrainingStart : MonoBehaviour
{
    [SerializeField] private GameObject _buttonSpace;
    [SerializeField] private GameObject _info1;
      

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {           
            _buttonSpace.SetActive(true);
            _info1.SetActive(true);            
            Time.timeScale = 0;
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Time.timeScale = 1;
            Destroy(this);            
        }
    }
}
