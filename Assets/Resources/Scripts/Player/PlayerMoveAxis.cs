using UnityEngine;
using Cinemachine;
using Zenject;

public class PlayerMoveAxis : MonoBehaviour
{
    [SerializeField, Tooltip("Used in SetForward method")] private Vector3 _forward = Vector3.forward;

    [Inject] private Player _player;

    private void Awake()
    {
        _forward.y = 0;

        if (_forward.sqrMagnitude == 0)
            throw new System.Exception("PlayerMoveAxis: Forward vector can't be zero");
    }

    public void SetDefault()
    {
        _player.Movement.CharacterController.Forward = Vector3.forward;
    }

    public void SetForward()
    {
        _forward.y = 0;
        _player.Movement.CharacterController.Forward = _forward.normalized;
    }

    public void SetFromCamera(CinemachineVirtualCamera camera)
    {
        Vector3 forward = camera.transform.forward;
        forward.y = 0;
        _player.Movement.CharacterController.Forward = forward.normalized;
    }
}
