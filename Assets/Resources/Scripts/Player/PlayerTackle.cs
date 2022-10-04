using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTackle : MonoBehaviour
{
    public KeyCode TackleButton = KeyCode.LeftControl;

    [SerializeField] private Vector3 _newColliderSize;
    private bool _isTackling = false;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(TackleButton) && _isTackling == false)
        {
            _isTackling = true;
            print("Left control");
        }

        if (_isTackling)
        {
            Vector3 v3Velocity = rb.velocity;
            
            if (v3Velocity.x == 0)
            {
                _isTackling = false;
            }
        }
    }
}
