using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorDigitalLock : MonoBehaviour, Interfaces.IInteractableMonoBehaviour
{
    [SerializeField] private GameObject _digitalLockGameObject; 
    
    public void Interact(Transform obj)
    {
        Time.timeScale = 0;
        var player = obj.GetComponent<PlayerMovement>();
        if (player == null) return;
        
        _digitalLockGameObject.SetActive(true);
    }
}
