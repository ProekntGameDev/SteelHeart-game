using UnityEngine;

namespace NewPlayerController
{
    public interface IControllable
    {
        void Move(float x, float z, bool isShift);
        void Jump();
        void HalfSquat();
        void RiseUp();
    }
}
