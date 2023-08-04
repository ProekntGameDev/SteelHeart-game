using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaveSystem;

public class SaveObject : MonoBehaviour
{
    [SerializeField] private int _index = 0;
    [SerializeField] private bool _isUse = false;

    private void Awake()
    {
        _isUse = SaveManager.SaveObjectInteractor.GetSaveObjectIsUse(_index);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_isUse && other.gameObject.TryGetComponent(out Player player))
        {
            _isUse = true;
            SaveManager.SaveObjectInteractor.SetSaveObjectIsUse(_index, _isUse);
            SaveManager.Save();
        }
    }
}
