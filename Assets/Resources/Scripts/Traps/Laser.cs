using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{    
    private LineRenderer _laser;

    private void Start()
    {
        _laser = GetComponent<LineRenderer>();
    }

    void Update ()
    {
        _laser.SetPosition(0, transform.position);
        RaycastHit hit;
        
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {            
            if (hit.collider)
            {
                _laser.SetPosition(1, hit.point);
                if (hit.collider.CompareTag("Player"))
                {
                    hit.collider.GetComponent<Player>().Health.Kill();                  
                }
            }
        }
        else _laser.SetPosition(1, transform.forward * 5000);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, transform.forward);
    }

}
