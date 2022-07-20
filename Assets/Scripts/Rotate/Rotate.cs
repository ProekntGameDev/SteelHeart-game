using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float RotateSpeed;

    // Update is called once per frame
    void FixedUpdate()
    {
         float angle = transform.eulerAngles.y;
         transform.Rotate(0,RotateSpeed * 1f * Time.deltaTime,0);
    }
}
