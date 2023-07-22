using UnityEngine;
using NaughtyAttributes;

public class PlayerInput : MonoBehaviour
{
    public Vector3 Axis { get; private set; }
    public bool IsJump { get; private set; }
    public bool IsRunning { get; private set; }
    public bool IsCrouching { get; private set; }
    public bool Interact { get; private set; }
    public bool Journal { get; private set; }

    [SerializeField, InputAxis] private string _horizontalAxis;
    [SerializeField, InputAxis] private string _verticalAxis;
    [SerializeField, InputAxis] private string _runAxis;
    [SerializeField, InputAxis] private string _jumpAxis;
    [SerializeField, InputAxis] private string _crouchAxis;

    [SerializeField] private KeyCode _journalKey;
    [SerializeField] private KeyCode _interactionKey;

    public void Update()
    {
        float jump = Input.GetAxis(_jumpAxis);

        Axis = new Vector3(Input.GetAxis(_horizontalAxis), jump, Input.GetAxis(_verticalAxis));
        Axis.Normalize();

        IsRunning = Input.GetAxis(_runAxis) > 0;
        IsCrouching = Input.GetAxis(_crouchAxis) > 0;

        IsJump = jump > 0;

        Interact = Input.GetKeyDown(_interactionKey);
        Journal = Input.GetKeyDown(_journalKey);
    }
}
