using UnityEngine;

public class Bouncer : MonoBehaviour
{
    public float maxVelocity = 20;
    public float decay = 0.5f;
    public float acceleration = 1.2f;
    
    private void OnCollisionEnter(Collision collision)
    {
        var rigidbody = collision.gameObject.GetComponent<Rigidbody>();
        if (rigidbody == null) return;

        var jumpController = collision.gameObject.GetComponent<PlayerJumpController>();
        if (jumpController == null) return;

        float verticalVelocity = Mathf.Abs(jumpController.PreviousVelocityY);

        if (jumpController.IsJumpButtonPressed)
        {
            verticalVelocity *= acceleration;
            if (verticalVelocity > maxVelocity) verticalVelocity = maxVelocity;
        }
        else
        {
            verticalVelocity *= decay;
        }
        
        float x = rigidbody.velocity.x;
        float y = verticalVelocity;
        float z = rigidbody.velocity.z;
        rigidbody.velocity = new Vector3(x, y, z);        
    }
}
