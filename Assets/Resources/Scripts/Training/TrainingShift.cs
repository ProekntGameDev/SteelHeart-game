using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingShift : MonoBehaviour
{
    [SerializeField] private GameObject _buttonShift;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _buttonShift.SetActive(true);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(_buttonShift);
            Destroy(this);
        }
    }
}
