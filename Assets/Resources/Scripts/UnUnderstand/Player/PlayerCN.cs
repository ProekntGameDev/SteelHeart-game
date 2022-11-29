using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCN : MonoBehaviour
{
  
    public float speed = 2f;
    public float rotationSpeed = 10;
    public float jumpforce = 2f;

    public Transform groundChecker;
    public LayerMask notPlayerMask; 
  

    private Rigidbody _rigidbody;
    private Animator _animator;
    private CapsuleCollider _collider;

    private bool isGrounded;

    // Start is called before the first frame update
    public void Start()
    {
        _collider = GetComponent<CapsuleCollider>();
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
   public  void FixedUpdate()
    {

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 directionVector = new Vector3(h,0,v);
        if (directionVector.magnitude > Mathf.Abs(0.1f))
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(directionVector), Time.deltaTime * rotationSpeed);

        _animator.SetFloat("speed", Vector3.ClampMagnitude(directionVector, 1).magnitude);
        Vector3 moveDir = Vector3.ClampMagnitude(directionVector, 1 ) * speed;
        _rigidbody.velocity = new Vector3(moveDir.x, _rigidbody.velocity.y,moveDir.z);
        _rigidbody.angularVelocity = Vector3.zero;

        if(Input.GetKeyDown(KeyCode.Space))
        {
              Jump();
        }

        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }

          if(Input.GetKeyUp(KeyCode.LeftControl))
        {
            UnCrouch();
        }
        
        
        if(Physics.CheckSphere(groundChecker.position,0.3f,notPlayerMask))
          {
              _animator.SetBool("isInAir",false);
              isGrounded = true;
              
          }
          else 
          {
               _animator.SetBool("isInAir",true);
               isGrounded = false;
          }
    }

     public void Crouch()
     {
          if(isGrounded)
          {
                _animator.SetBool("isCrouching",true);
                speed = 1.2f;
                _collider.height = 1.4f;
                 _collider.center = new Vector3(_collider.center.x,0.55f,_collider.center.z);
          }

     }

     public void UnCrouch()
     {
         _animator.SetBool("isCrouching",false);
         speed = 4.5f;
         _collider.height = 1.9f;
         _collider.center = new Vector3(_collider.center.x,0.9603896f,_collider.center.z);
     }

    public void Jump()  
    {
         if(_animator.GetBool("isCrouching")) return;
          _animator.SetTrigger("Jump");
          if(isGrounded)
          {
                 _rigidbody.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
          }
    }
}
    
