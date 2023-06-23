using UnityEngine;

namespace NewPlayerController
{
    public class RunPlayerBehaviour : MonoBehaviour, IPlayerBehaviour
    {
        public bool IsActive { get; private set; } //activity behavior
        public IPlayerBehaviourData PlayerData { get; private set; } //player data

        [SerializeField] private float _runSpeed = 6f; //running speed

        private void Awake()
        {
            PlayerData = GetComponent<IPlayerBehaviourData>(); //receiving IPlayerBehaviourData
        }

        public void EnterBehaviour()
        {
            IsActive = true;
            PlayerData.SpeedPlayer = _runSpeed; //set the speed
        }
        public void UpdateBehaviour()
        {
            Run(); //player run
        }
        public void ExitBehaviour()
        {
            IsActive = false;
        }

        private void Run()
        {
            if (PlayerData != null && PlayerData.PlayerMovement != null)
                PlayerData.PlayerMovement.MovePlayer(PlayerData.Z, PlayerData.X, PlayerData.SpeedPlayer, PlayerData, 0);
        }
    }
}
