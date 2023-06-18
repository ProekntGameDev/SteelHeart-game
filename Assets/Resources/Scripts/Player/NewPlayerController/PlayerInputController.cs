using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IControllable))]
public class PlayerInputController : MonoBehaviour
{
    private IControllable _controllable;

    private void Awake()
    {
        _controllable = GetComponent<IControllable>();
    }

    private void Update()
    {
        _controllable.Move(Input.GetAxis("Horizontal"), -Input.GetAxis("Vertical"), Input.GetKey(KeyCode.LeftShift));
        if (Input.GetKeyDown(KeyCode.Space)) _controllable.Jump();
    }
}
