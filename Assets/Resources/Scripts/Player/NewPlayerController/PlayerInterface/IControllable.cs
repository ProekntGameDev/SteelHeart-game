using UnityEngine;

public interface IControllable
{
    void Move(float x, float z, bool isShift);
    void Jump();
}
