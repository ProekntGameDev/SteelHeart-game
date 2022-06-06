using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int _speed = 10;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        var horizontalAxis = Input.GetAxis("Horizontal");
        _rigidbody.velocity = new Vector3(horizontalAxis * _speed, 0, 0);

        if (_rigidbody.velocity.x < -.01)
            transform.eulerAngles = new Vector3(0, 180, 0);
        else if (_rigidbody.velocity.x > .01)
            transform.eulerAngles = new Vector3(0, 0, 0);

    }
}
