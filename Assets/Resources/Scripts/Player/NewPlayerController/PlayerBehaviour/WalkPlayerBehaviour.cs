//using UnityEngine;

namespace NewPlayerController
{
    public class WalkPlayerBehaviour : IPlayerBehaviour
    {
        public IPlayerBehaviourData PlayerData { get; private set; } //player data

        private PlayerBehaviourController _playerBehaviourController;

        private float _walkSpeed = 3f; //walking speed

        public WalkPlayerBehaviour(IPlayerBehaviourData playerData, PlayerBehaviourController playerBehaviourController, float walkSpeed)
        {
            PlayerData = playerData;
            _playerBehaviourController = playerBehaviourController;
            _walkSpeed = walkSpeed;
        }

        public void EnterBehaviour()
        {
            if (PlayerData != null && PlayerData.SpeedPlayer != null) PlayerData.SpeedPlayer.SetSpeed(_walkSpeed); //set the speed
        }

        public void UpdateBehaviour()
        {
            Walk(); //player walk
            CheckIsGrounded();
            CheckPlayerIdle();
        }

        public void ExitBehaviour() { }

        public void SetNewBehaviour<T>() where T : IPlayerBehaviour
        {
            if (typeof(T) == typeof(RunPlayerBehaviour) || typeof(T) == typeof(JumpPlayerBehaviour) || typeof(T) == typeof(HalfSquatPlayerBehaviour))
                _playerBehaviourController.SetBehaviour<T>();
        }

        private void Walk()
        {
            if (PlayerData != null && PlayerData.PlayerMovement != null)
                PlayerData.PlayerMovement.MovePlayer(PlayerData.Z, PlayerData.X, PlayerData.SpeedPlayer.Speed, PlayerData, 0);
        }

        private void CheckIsGrounded()
        {
            if (PlayerData != null && !PlayerData.IsGrounded)
                _playerBehaviourController.SetBehaviour<FallPlayerBehaviour>();
        }

        private void CheckPlayerIdle()
        {
            if (PlayerData.X == 0 && PlayerData.Z == 0) //if the player idle
                _playerBehaviourController.SetBehaviour<IdlePlayerBehaviour>();
        }
    }
}