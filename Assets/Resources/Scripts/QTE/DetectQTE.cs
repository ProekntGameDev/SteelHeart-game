using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectQTE : MonoBehaviour
{
    private bool PlayerAtObject=false;
    private bool QTEend=false;
    private bool QTEstart=false;
    public KeyCode Button;
    private float QTEcount;
    [Range(0f,10f)]public float QTEdifficult;
    [Range(0f,10f)]public float QTErollback;
    [Range(0f,10f)]public float QTEforclick;
    
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Player")PlayerAtObject=true;   
    }
    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag=="Player")PlayerAtObject=false;   
    }
    void Start()
    {
        PlayerPrefs.SetFloat("QTEdifficult",QTEdifficult);       
    }
    void Update() 
    {
        QTEstart=(QTEstart||Input.GetKey(Button))&!QTEend;
        if(QTEstart)
        {
            if(Input.GetKeyDown(Button))QTEcount+=QTEforclick;
            if(QTEcount>=QTEdifficult)QTEend=true;
            QTEcount-=QTErollback*Time.deltaTime;   
        }
        PlayerPrefs.SetFloat("QTEcount",QTEcount);
    }

}

