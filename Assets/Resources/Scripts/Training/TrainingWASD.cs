using UnityEngine;

public class TrainingWASD : MonoBehaviour
{
    [SerializeField] private GameObject _buttonWASD;
    private bool _buttonWasdWasDestroyed;


    private void OnTriggerEnter(Collider other)
    {
        if (!_buttonWasdWasDestroyed && other.CompareTag("Player")) 
            _buttonWASD.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (_buttonWasdWasDestroyed || !other.CompareTag("Player")) return;

        Destroy(_buttonWASD);
        _buttonWasdWasDestroyed = true;
    }
}