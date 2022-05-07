using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform target;
    public Vector3 offset;

    private void FixedUpdate()
    {
        if(target)
            transform.position = target.position - offset;
    }
}
