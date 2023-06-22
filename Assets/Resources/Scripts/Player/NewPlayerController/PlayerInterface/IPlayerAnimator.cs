using UnityEngine;

namespace NewPlayerController
{
    public interface IPlayerAnimator
    {
        Animator Animator { get; }
        string IsInAir { get; }
        string IsCrouching { get; }
        string Speed { get; }
        string Jump { get; }
    }
}
