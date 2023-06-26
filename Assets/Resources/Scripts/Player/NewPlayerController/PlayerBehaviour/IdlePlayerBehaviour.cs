using UnityEngine;

namespace NewPlayerController
{
    public class IdlePlayerBehaviour : IPlayerBehaviour
    {
        public IPlayerBehaviourData PlayerData { get; private set; } //player data

        private PlayerBehaviourController _playerBehaviourController;

        public IdlePlayerBehaviour(IPlayerBehaviourData playerData, PlayerBehaviourController playerBehaviourController)
        {
            PlayerData = playerData;
            _playerBehaviourController = playerBehaviourController;
        }

        public void EnterBehaviour()
        {
            if (PlayerData != null && PlayerData.SpeedPlayer != null) PlayerData.SpeedPlayer.SetSpeed(0f); //set the speed
        }
        public void UpdateBehaviour()
        {
            CheckPlayerWalk();
        }
        public void ExitBehaviour() { }

        public void SetNewBehaviour<T>() where T : IPlayerBehaviour
        {
            if (typeof(T) == typeof(FallPlayerBehaviour)) return;
            _playerBehaviourController.SetBehaviour<T>();
        }

        private void CheckPlayerWalk()
        {
            if (PlayerData.X != 0 || PlayerData.Z != 0) //if the player moves
                _playerBehaviourController.SetBehaviour<WalkPlayerBehaviour>();
        }
    }
}
