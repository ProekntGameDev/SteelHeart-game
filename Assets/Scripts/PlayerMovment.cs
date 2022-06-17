using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovment : MonoBehaviour
{
    [SerializeField] private int speed = 10;
    private Rigidbody rb;
    public bool isGrounded;


    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update ()
    {
        Jump();
    }

    // Update is called once per frame
    void FixedUpdate()
     { 
       move();
     }

     void move() 
     {
         rb.velocity = new Vector3(Input.GetAxis("Horizontal") * speed, rb.velocity.y, Input.GetAxis("Vertical") * speed);//move

        
        if (rb.velocity.x < -.01)// flip
            transform.eulerAngles = new Vector3(0, 180, 0);
        else if (rb.velocity.x > .01)
            transform.eulerAngles = new Vector3(0, 0, 0);

     }
     
     void Jump()
   
     {
        if (Input.GetButtonDown("Jump") && isGrounded)//jump
            {
                isGrounded = false;
                rb.AddForce(new Vector3(0 , 300 , 0));
            }

    }

     void OnCollisionEnter()
     
    { //ground

         isGrounded = true;
        
    }

    

   
}
