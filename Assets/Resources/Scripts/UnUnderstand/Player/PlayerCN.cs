using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCN : MonoBehaviour
{
  
    public float speed = 2f;
    public float rotationSpeed = 10;

    private Rigidbody _rigidbody;
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 directionVector = new Vector3(h,0,v);
        if (directionVector.magnitude > Mathf.Abs(0.1f))
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(directionVector), Time.deltaTime * 10);

        _animator.SetFloat("speed", Vector3.ClampMagnitude(directionVector, 1).magnitude);
        _rigidbody.velocity = Vector3.ClampMagnitude(directionVector,1) * speed;
        _rigidbody.angularVelocity = Vector3.zero;
        
    }
}
