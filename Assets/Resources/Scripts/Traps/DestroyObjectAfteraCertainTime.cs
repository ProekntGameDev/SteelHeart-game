using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjectAfteraCertainTime : MonoBehaviour
{    
    [SerializeField] private float timeAfterDestroy; 
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {                   
            StartCoroutine(CountdownToDestruction());
        }
    }    

    IEnumerator CountdownToDestruction()
    {
        yield return new WaitForSeconds(timeAfterDestroy);
        Destroy(this.gameObject);
        
    }
}
