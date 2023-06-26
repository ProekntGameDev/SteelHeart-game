using UnityEngine;
using System;

namespace NewPlayerController
{
    public class RisePlayerBehaviour : IPlayerBehaviour
    {
        public static event Func<Transform> OnGetTransformOfladder;

        public IPlayerBehaviourData PlayerData { get; private set; } //player data

        private PlayerBehaviourController _playerBehaviourController;

        private float _riseSpeed = 3f;

        private Transform _transformLadder;
        private bool _isPointLadder = false;

        private const float MinDistance = 1f;

        public RisePlayerBehaviour(IPlayerBehaviourData playerData, PlayerBehaviourController playerBehaviourController, float riseSpeed)
        {
            PlayerData = playerData;
            _playerBehaviourController = playerBehaviourController;
            _riseSpeed = riseSpeed;
        }

        public void EnterBehaviour()
        {
            _transformLadder = OnGetTransformOfladder?.Invoke();
            if (PlayerData != null && PlayerData.SpeedPlayer != null) PlayerData.SpeedPlayer.SetSpeed(_riseSpeed);
        }
        public void UpdateBehaviour()
        {
            if (Vector3.Distance(PlayerData.TransformPlayer.position, _transformLadder.position) > MinDistance)
            {
                if (PlayerData != null && PlayerData.PlayerMovement != null)
                    PlayerData.PlayerMovement.LerpPlayer(PlayerData.TransformPlayer.position, _transformLadder.position, 1f);
            }
            RiseUp();
        }

        public void ExitBehaviour() { }

        public void SetNewBehaviour<T>() where T : IPlayerBehaviour
        {
            if (typeof(T) == typeof(RisePlayerBehaviour))
                _playerBehaviourController.SetBehaviour<IdlePlayerBehaviour>();
        }

        private void RiseUp()
        {
            if (PlayerData != null && PlayerData.PlayerMovement != null)
                PlayerData.PlayerMovement.MoveYPlayer(PlayerData, -PlayerData.Z, PlayerData.SpeedPlayer.Speed);
        }
    }
}
