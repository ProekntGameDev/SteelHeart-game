using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IControllable))]
public class PlayerInputController : MonoBehaviour
{
    private IControllable _controllable; //managed object

    private void Awake()
    {
        _controllable = GetComponent<IControllable>(); //receiving IControllable
    }

    private void Update()
    {
        _controllable.Move(Input.GetAxis("Horizontal"), -Input.GetAxis("Vertical"), Input.GetKey(KeyCode.LeftShift)); //motion input
        if (Input.GetKeyDown(KeyCode.Space)) _controllable.Jump(); //jump activation
    }
}
