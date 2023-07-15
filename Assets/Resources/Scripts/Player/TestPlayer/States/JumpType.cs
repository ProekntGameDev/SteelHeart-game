using UnityEngine;

[System.Serializable]
public class JumpType
{
    public float InitalJumpForce => _initalJumpForce;
    public AnimationCurve GravityRise => _gravityRise;
    public float GravityOnRelease => _gravityOnRelease;

    [SerializeField] private float _initalJumpForce;
    [SerializeField] private AnimationCurve _gravityRise;
    [SerializeField] private float _gravityOnRelease;
}
