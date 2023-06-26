using UnityEngine;

namespace NewPlayerController
{
    public class JumpPlayerBehaviour : IPlayerBehaviour
    {
        public IPlayerBehaviourData PlayerData { get; private set; } //player data

        private PlayerBehaviourController _playerBehaviourController;

        private float _jumpForce = 12f; //jump power
        private float _jumpSpeed;//speed with he fly

        private const float Gravity = -9.81f;//the acceleration with which it slows down

        public JumpPlayerBehaviour(IPlayerBehaviourData playerData, PlayerBehaviourController playerBehaviourController, float jumpForce)
        {
            PlayerData = playerData;
            _playerBehaviourController = playerBehaviourController;
            _jumpForce = jumpForce;
        }

        public void EnterBehaviour()
        {
            _jumpSpeed = _jumpForce;
        }

        public void UpdateBehaviour()
        {
            Jump(); //player jump
        }

        public void ExitBehaviour() { }

        public void SetNewBehaviour<T>() where T : IPlayerBehaviour { }

        private void Jump()
        {
            _jumpSpeed += Gravity * Time.deltaTime * 3f;

            if (PlayerData != null && PlayerData.PlayerMovement != null)
                PlayerData.PlayerMovement.JumpPlayer(PlayerData.Z, PlayerData.X, PlayerData.SpeedPlayer.Speed, PlayerData, _jumpSpeed);

            if (_jumpSpeed <= 0) _playerBehaviourController.SetBehaviour<FallPlayerBehaviour>();
        }
    }
}
