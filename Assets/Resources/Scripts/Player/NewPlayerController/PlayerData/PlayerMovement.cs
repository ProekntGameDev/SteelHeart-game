using UnityEngine;

namespace NewPlayerController
{
    public class PlayerMovement : MonoBehaviour
    {
        public Vector3 LerpPlayer(Vector3 player, Vector3 target, float speed)
        {
            return Vector3.Lerp(player, target, speed);
        }

        public void MoveYPlayer(IPlayerBehaviourData playerData, float y, float speed)
        {
            Vector3 direction = y * Vector3.up;
            direction.Normalize();

            direction *= speed * Time.deltaTime;

            playerData.CharacterController.Move(direction);
        }

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
            playerData.TransformPlayer.localRotation = Quaternion.LookRotation(forward, Vector3.up);
        }
    }
}

