using UnityEngine;

namespace NewPlayerController
{
    public class WalkPlayerBehaviour : MonoBehaviour, IPlayerBehaviour
    {
        public bool IsActive { get; private set; } //activity behavior
        public IPlayerBehaviourData PlayerData { get; private set; } //player data

        [SerializeField] private float _walkSpeed = 3f; //walking speed

        private void Awake()
        {
            PlayerData = GetComponent<IPlayerBehaviourData>(); //receiving IPlayerBehaviourData
        }

        public void EnterBehaviour()
        {
            IsActive = true;
            PlayerData.SpeedPlayer = _walkSpeed; //set the speed
        }
        public void UpdateBehaviour()
        {
            Walk(); //player walk
        }
        public void ExitBehaviour()
        {
            IsActive = false;
        }

        private void Walk()
        {
            if (PlayerData != null && PlayerData.PlayerMovement != null)
                PlayerData.PlayerMovement.MovePlayer(PlayerData.Z, PlayerData.X, PlayerData.SpeedPlayer, PlayerData, 0);
        }
    }
}
