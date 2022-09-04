using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNightvisionAbility : MonoBehaviour
{
    public KeyCode nightvisionKey = KeyCode.N;

    private bool _isActive;

    private void Update()
    {
        if (Input.GetKeyDown(nightvisionKey))
        {
            Camera.main.GetComponent<CameraController>().NightVisionEffectActiveStateChange();
        }

        /*
        if (_isActive)
        {
            Camera.main.gameObject.GetComponent<CameraController>().NightVisionEffectActiveStateChange();
            _isActive = !_isActive;
        }
        else
        {
            Camera.main.gameObject.GetComponent<CameraController>().NightVisionEffectActiveStateChange();
            _isActive = !_isActive;
        }
        */
        
    }
}
