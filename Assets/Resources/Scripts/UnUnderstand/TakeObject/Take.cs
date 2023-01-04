using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Take : MonoBehaviour
{

     float distanse = 3f;
     public Transform transformPos;
     private Rigidbody _rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody=GetComponent<Rigidbody>();
    }

    public void OnTake()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            _rigidbody.isKinematic = true;
            _rigidbody.MovePosition(transformPos.position);
        }
    }
    public void FixedUpdate()
    {
        if (_rigidbody.isKinematic == true)
        {
            this.gameObject.transform.position = transformPos.position;
            if (Input.GetKeyDown(KeyCode.G))
            {
                _rigidbody.useGravity = true;
                _rigidbody.isKinematic = false;
                _rigidbody.AddForce(Camera.main.transform.position * 100);
            }
        }
    }
}
