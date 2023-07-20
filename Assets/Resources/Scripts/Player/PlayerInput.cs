using UnityEngine;
using NaughtyAttributes;

public class PlayerInput : MonoBehaviour
{
    public Vector3 input { get; private set; }
    public bool IsJump { get; private set; }
    public bool IsRunning { get; private set; }
    public bool IsCrouching { get; private set; }

    [SerializeField, InputAxis] private string _horizontalAxis;
    [SerializeField, InputAxis] private string _verticalAxis;
    [SerializeField, InputAxis] private string _runAxis;
    [SerializeField, InputAxis] private string _jumpAxis;
    [SerializeField, InputAxis] private string _crouchAxis;

    public void Update()
    {
        float jump = Input.GetAxis(_jumpAxis);

        input = new Vector3(Input.GetAxis(_horizontalAxis), jump, Input.GetAxis(_verticalAxis));
        input.Normalize();

        IsRunning = Input.GetAxis(_runAxis) > 0;
        IsCrouching = Input.GetAxis(_crouchAxis) > 0;

        IsJump = jump > 0;
    }
}
