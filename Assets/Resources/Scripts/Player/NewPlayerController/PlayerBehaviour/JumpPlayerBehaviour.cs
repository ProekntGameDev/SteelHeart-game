using UnityEngine;

namespace NewPlayerController
{
    public class JumpPlayerBehaviour : MonoBehaviour, IPlayerBehaviour
    {
        public bool IsActive { get; private set; } //activity behavior
        public IPlayerBehaviourData PlayerData { get; private set; } //player data

        [SerializeField] private float _jumpForce = 12f; //jump power

        private const float Gravity = -9.81f;//the acceleration with which it slows down

        private float _jumpSpeed;//speed with he fly

        private void Awake()
        {
            PlayerData = GetComponent<IPlayerBehaviourData>(); //receiving IPlayerBehaviourData
        }

        public void EnterBehaviour()
        {
            IsActive = true;
            _jumpSpeed = _jumpForce;
        }
        public void UpdateBehaviour()
        {
            Jump(); //player jump
        }
        public void ExitBehaviour()
        {
            IsActive = false;
        }

        private void Jump()
        {
            _jumpSpeed += Gravity * Time.deltaTime * 3f;

            if (PlayerData != null && PlayerData.PlayerMovement != null)
                PlayerData.PlayerMovement.JumpPlayer(PlayerData.Z, PlayerData.X, PlayerData.SpeedPlayer, PlayerData, _jumpSpeed);

            if (_jumpSpeed <= 0) IsActive = false;
        }
    }
}
