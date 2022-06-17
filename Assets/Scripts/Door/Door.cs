using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
        public Transform PlayerCamera;
        [Header("MaxDistance you can open or close the door.")]
         public float MaxDistance = 5;

        private bool _isOpended = false;
        private Animator _animator;    
   

       void Update()
    {
        //This will tell if the player press F on the Keyboard. P.S. You can change the key if you want.
        if (Input.GetKeyDown(KeyCode.F))
        {
            Pressed();
            //Delete if you dont want Text in the Console saying that You Press F.
            Debug.Log("You Press F");
        }
    }

   public void Pressed(){

            RaycastHit doorhit;

        if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out doorhit, MaxDistance))
        {
 
            // if raycast hits, then it checks if it hit an object with the tag Door.
             if (doorhit.transform.tag == "Door")
            {
             //This line will get the Animator from  Parent of the door that was hit by the raycast.
            _animator = doorhit.transform.GetComponentInParent<Animator>();
       //This line will set the bool true so it will play the animation.
             _animator.SetBool("isOpended", _isOpended);
        //This will set the bool the opposite of what it is.
             _isOpended = !_isOpended;
           }
        }
    }
 
 }   

