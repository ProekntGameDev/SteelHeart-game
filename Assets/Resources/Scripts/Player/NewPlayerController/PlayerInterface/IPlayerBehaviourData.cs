using UnityEngine;

namespace NewPlayerController
{
    public interface IPlayerBehaviourData
    {
        CharacterController CharacterController { get; }
        Transform TransformPlayer { get; }
        float X { get; }
        float Z { get; }
        PlayerSpeed SpeedPlayer { get; }
        IPlayerAnimator PlayerAnimator { get; }
        PlayerMovement PlayerMovement { get; }
        bool IsGrounded { get; }
    }
}
