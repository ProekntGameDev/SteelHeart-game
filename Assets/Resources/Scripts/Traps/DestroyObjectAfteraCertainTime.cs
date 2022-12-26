using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjectAfteraCertainTime : MonoBehaviour
{    
    [SerializeField] private float timeAfterDestroy = 1f;
    private bool destroy = false;    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            destroy = true;           
        }
    }
    private void Update()
    {   
        if(destroy)
        StartCoroutine(CountdownToDestruction());
    }


    IEnumerator CountdownToDestruction()
    {
        yield return new WaitForSeconds(timeAfterDestroy);
        Destroy(this.gameObject);
        
    }
}
