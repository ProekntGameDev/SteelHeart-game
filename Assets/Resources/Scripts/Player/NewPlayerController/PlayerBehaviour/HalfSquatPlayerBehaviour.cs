using UnityEngine;

namespace NewPlayerController
{
    public class HalfSquatPlayerBehaviour : MonoBehaviour, IPlayerBehaviour
    {
        public bool IsActive { get; private set; } //activity behavior
        public IPlayerBehaviourData PlayerData { get; private set; } //player data

        [SerializeField] private float _halfSquatSpeed = 2f; //speed in a half-squat

        private float _heightPlayer; //player height in CharacterController
        private Vector3 _playerCenter; //player center in CharacterController

        private float _currentHeight = 1.3f; //current player height in CharacterController
        private Vector3 _currentCenter = new Vector3(0, 0.7f, 0); //current player center in CharacterController

        private void Awake()
        {
            PlayerData = GetComponent<IPlayerBehaviourData>(); //receiving IPlayerBehaviourData
        }

        public void EnterBehaviour()
        {
            IsActive = true;
            PlayerData.SpeedPlayer = _halfSquatSpeed; //set the speed
            _heightPlayer = PlayerData.CharacterController.height; //get player height
            _playerCenter = PlayerData.CharacterController.center; //get player center
            PlayerData.CharacterController.height = _currentHeight; //set current player height
            PlayerData.CharacterController.center = _currentCenter; //set current player center
        }
        public void UpdateBehaviour()
        {
            WalkHalfSquat(); //the player moves on a half-squat
        }
        public void ExitBehaviour()
        {
            PlayerData.CharacterController.height = _heightPlayer; //set player height
            PlayerData.CharacterController.center = _playerCenter; //set player center
            IsActive = false;
        }

        private void WalkHalfSquat()
        {
            if (PlayerData != null && PlayerData.PlayerMovement != null)
                PlayerData.PlayerMovement.MovePlayer(PlayerData.Z, PlayerData.X, PlayerData.SpeedPlayer, PlayerData, 0);
        }
    }
}
