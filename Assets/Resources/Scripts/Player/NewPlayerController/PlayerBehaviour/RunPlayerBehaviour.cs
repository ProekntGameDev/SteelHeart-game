using UnityEngine;

namespace NewPlayerController
{
    public class RunPlayerBehaviour : IPlayerBehaviour
    {
        public IPlayerBehaviourData PlayerData { get; private set; } //player data

        private PlayerBehaviourController _playerBehaviourController;

        private float _runSpeed = 6f; //running speed

        public RunPlayerBehaviour(IPlayerBehaviourData playerData, PlayerBehaviourController playerBehaviourController, float runSpeed)
        {
            PlayerData = playerData;
            _playerBehaviourController = playerBehaviourController;
            _runSpeed = runSpeed;
        }

        public void EnterBehaviour()
        {
            if (PlayerData != null && PlayerData.SpeedPlayer != null) PlayerData.SpeedPlayer.SetSpeed(_runSpeed); //set the speed
        }

        public void UpdateBehaviour()
        {
            Run(); //player run
            CheckIsGrounded();
            CheckPlayerIdle();
        }

        public void ExitBehaviour() { }

        public void SetNewBehaviour<T>() where T : IPlayerBehaviour
        {
            if (typeof(T) == typeof(WalkPlayerBehaviour) || typeof(T) == typeof(JumpPlayerBehaviour) || typeof(T) == typeof(HalfSquatPlayerBehaviour))
                _playerBehaviourController.SetBehaviour<T>();
        }

        private void Run()
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
