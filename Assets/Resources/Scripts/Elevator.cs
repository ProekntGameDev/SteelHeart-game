using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

public class Elevator : MonoBehaviour, IInteractableMonoBehaviour
{
    public float lift_height = 4;
    public float lift_speed = 1f;
    public bool isUpPosition = false;

    private int direction = 1;
    private float defaultHeight;
    private bool isActive = false;
    private PlayerMovement movementComponent;
    private Transform player;

    private void Start()
    {
        direction = (isUpPosition) ? -1 : 1;
        defaultHeight = transform.position.y + lift_height * 0.5f * direction;
    }
    void FixedUpdate()
    {
        if (isActive) 
        {
            Vector3 lift = Vector3.up * direction * lift_speed * Time.fixedDeltaTime;
            transform.position += lift;
            player.position += lift;
            if (Mathf.Abs(transform.position.y - defaultHeight) > lift_height * 0.5f)
            {
                direction *= -1;
                isActive = false;
                //movementComponent.isWalkingAllowed = true;
            }
        }
    }
    public void Interact(Transform obj)
    {
        player = obj;
        movementComponent = obj.GetComponent<PlayerMovement>();
        //if (movementComponent.IsOnFloor)
        //{
        //    movementComponent.isWalkingAllowed = false;
        //    isActive = true;
        //}
    }
}
