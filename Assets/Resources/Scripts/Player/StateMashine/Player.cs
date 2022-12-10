using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody rigidbody;
    public Animator animator;
    public CapsuleCollider collider;
    public Transform target;

    
    public void Start()
    {

        collider = GetComponent<CapsuleCollider>();
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        
    }

    
}
