using UnityEngine;

public interface IPlayerBehaviourData
{
    CharacterController CharacterController { get; }
    Transform TransformPlayer { get; }
    float X { get; }
    float Z { get; }
    float SpeedPlayer { get; }
    IPlayerAnimator PlayerAnimator { get; }
}
