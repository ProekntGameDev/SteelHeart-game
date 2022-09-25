using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorDigitalLock : MonoBehaviour, IInteractableMonoBehaviour
{
    [SerializeField] private GameObject _digitalLockGameObject; 
    
    public void Interact(Transform obj)
    {
        var player = obj.GetComponent<PlayerMovement>();
        if (player == null) return;
        
        _digitalLockGameObject.SetActive(true);
    }
}
