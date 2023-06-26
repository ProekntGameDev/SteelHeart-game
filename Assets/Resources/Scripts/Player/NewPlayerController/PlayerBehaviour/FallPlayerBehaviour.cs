using UnityEngine;

namespace NewPlayerController
{
    public class FallPlayerBehaviour : IPlayerBehaviour
    {
        public bool IsActive { get; private set; } //activity behavior
        public IPlayerBehaviourData PlayerData { get; private set; } //player data

        private PlayerBehaviourController _playerBehaviourController;

        private float _gravity; //gravity

        public FallPlayerBehaviour(IPlayerBehaviourData playerData, PlayerBehaviourController playerBehaviourController, float gravity)
        {
            PlayerData = playerData;
            _playerBehaviourController = playerBehaviourController;
            _gravity = gravity;
        }

        public void EnterBehaviour() { }
        public void UpdateBehaviour()
        {
            Fall(); //player fall
            CheckIsGrounded();
        }
        public void ExitBehaviour() { }

        public void SetNewBehaviour<T>() where T : IPlayerBehaviour { }

        private void Fall()
        {
            if (PlayerData != null && PlayerData.PlayerMovement != null)
                PlayerData.PlayerMovement.MovePlayer(PlayerData.Z, PlayerData.X, PlayerData.SpeedPlayer.Speed, PlayerData, _gravity);
        }

        private void CheckIsGrounded()
        {
            if (PlayerData != null && PlayerData.IsGrounded)
                _playerBehaviourController.SetBehaviour<IdlePlayerBehaviour>();
        }
    }
}

