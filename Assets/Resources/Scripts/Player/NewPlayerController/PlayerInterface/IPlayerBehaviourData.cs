using UnityEngine;

namespace NewPlayerController
{
    public interface IPlayerBehaviourData
    {
        CharacterController CharacterController { get; }
        Transform TransformPlayer { get; }
        float X { get; }
        float Z { get; }
        float SpeedPlayer { get; set; }
        IPlayerAnimator PlayerAnimator { get; }
        PlayerMovement PlayerMovement { get; }
    }
}
