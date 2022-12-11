using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody rigidbody;
    public Animator animator;
    public CapsuleCollider collider;
    public Transform target;

    public bool isGround => collisionCount > 0;
    private int collisionCount = 0;
    
    public void Start()
    {
        collider = GetComponent<CapsuleCollider>();
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
    }

    private List<string> ignoredTags = new List<string>() {  };

    private void OnTriggerEnter(Collider other)
    {
        if(!ignoredTags.Contains(other.tag))
            collisionCount++;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!ignoredTags.Contains(other.tag))
            collisionCount--;
    }
}
