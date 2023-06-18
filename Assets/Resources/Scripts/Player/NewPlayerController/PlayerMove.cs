using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public void MovePlayer(float z, float x, float speed, IPlayerBehaviourData playerData, float y)
    {
        Vector3 direction = z * Vector3.back + x * Vector3.right;
        direction.Normalize();
        
        if (direction.sqrMagnitude != 0) Rotate(direction, playerData);

        direction *= speed * Time.deltaTime;
        direction += new Vector3(0, y, 0);

        playerData.CharacterController.Move(direction);
    }

    public void JumpPlayer(float z, float x, float speed, IPlayerBehaviourData playerData, float y)
    {
        Vector3 direction = z * Vector3.back * speed + x * Vector3.right * speed;
        direction.Normalize();


        if (direction.sqrMagnitude != 0) Rotate(direction, playerData);

        direction += new Vector3(0, y, 0);
        playerData.CharacterController.Move(direction * Time.deltaTime);
    }

    private void Rotate(Vector3 forward, IPlayerBehaviourData playerData)
    {
        playerData.PositionPlayer.localRotation = Quaternion.LookRotation(forward, Vector3.up);
    }
}
