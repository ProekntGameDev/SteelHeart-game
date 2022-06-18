using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour {
     
     public GameObject GrapplingHook;
     private GameObject auxGraplingHook;

     public KeyCode KeyGrappelr;

     public Camera _camera;

     public Transform  dirDoubleclick;
     private Transform auxDirDoubleclick;

     private Vector3 localDoubleclick;
     private Vector3 posMouse;
     private Quaternion lookAtDir;
 
 
    void Update()
    {

         posMouse = Input.mousePosition;
       
         posMouse.z = Vector3.Distance(_camera.transform.position , transform.position);
         posMouse = _camera.ScreenToWorldPoint(posMouse);

       if(auxGraplingHook == null){
            
         if(Input.GetMouseButtonDown(0)){
       
             auxDirDoubleclick = Instantiate(dirDoubleclick , posMouse , Quaternion.identity) as Transform;
             localDoubleclick = (auxDirDoubleclick.transform.position - transform.position).normalized;
             lookAtDir = Quaternion.LookRotation(localDoubleclick);

             auxGraplingHook = Instantiate(GrapplingHook , transform.position , lookAtDir) as GameObject;
             Destroy(auxDirDoubleclick.gameObject);
         }
       }
    }
}
