using UnityEngine;

namespace NewPlayerController
{
    public class HalfSquatPlayerBehaviour : IPlayerBehaviour
    {
        public IPlayerBehaviourData PlayerData { get; private set; } //player data

        private PlayerBehaviourController _playerBehaviourController;

        private float _halfSquatSpeed = 2f; //speed in a half-squat

        private float _heightPlayer; //player height in CharacterController
        private Vector3 _playerCenter; //player center in CharacterController

        private float _currentHeight = 1.3f; //current player height in CharacterController
        private Vector3 _currentCenter = new Vector3(0, 0.7f, 0); //current player center in CharacterController

        public HalfSquatPlayerBehaviour(IPlayerBehaviourData playerData, PlayerBehaviourController playerBehaviourController, float halfSquatSpeed)
        {
            PlayerData = playerData;
            _playerBehaviourController = playerBehaviourController;
            _halfSquatSpeed = halfSquatSpeed;
        }

        public void EnterBehaviour()
        {
            if(PlayerData != null)
            {
                if (PlayerData.SpeedPlayer != null) PlayerData.SpeedPlayer.SetSpeed(_halfSquatSpeed); //set the speed
                if (PlayerData.CharacterController != null)
                {
                    _heightPlayer = PlayerData.CharacterController.height; //get player height
                    _playerCenter = PlayerData.CharacterController.center; //get player center
                    PlayerData.CharacterController.height = _currentHeight; //set current player height
                    PlayerData.CharacterController.center = _currentCenter; //set current player center
                }
            }
        }
        public void UpdateBehaviour()
        {
            WalkHalfSquat(); //the player moves on a half-squat
        }
        public void ExitBehaviour()
        {
            if (PlayerData != null && PlayerData.CharacterController != null)
            {
                PlayerData.CharacterController.height = _heightPlayer; //set player height
                PlayerData.CharacterController.center = _playerCenter; //set player center
            }
        }

        public void SetNewBehaviour<T>() where T : IPlayerBehaviour
        {
            if (typeof(T) == typeof(HalfSquatPlayerBehaviour))
                _playerBehaviourController.SetBehaviour<IdlePlayerBehaviour>();
        }

        private void WalkHalfSquat()
        {
            if (PlayerData != null && PlayerData.PlayerMovement != null)
                PlayerData.PlayerMovement.MovePlayer(PlayerData.Z, PlayerData.X, PlayerData.SpeedPlayer.Speed, PlayerData, 0);
        }
    }
}
