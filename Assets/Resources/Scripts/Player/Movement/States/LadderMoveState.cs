using UnityEngine;

public class LadderMoveState : MoveState
{
    private Transform _playerLadderTrigger;
    private Ladder _ladder;
    private Collider _ladderCollider;
    private float _speed;

    public LadderMoveState(InertialCharacterController characterController, Transform playerLadderTrigger, float speed) : base(characterController, 0, 0)
    {
        _speed = speed;
        _playerLadderTrigger = playerLadderTrigger;
    }

    public override void OnEnter()
    {
        if(_ladder == null)
            throw new System.NullReferenceException(nameof(_ladder));

        _characterController.CurrentVelocity = Vector3.zero;
        _characterController.VerticalVelocity = 0;

        _characterController.VerticalMove = false;
    }

    public void Init(Ladder ladder)
    {
        _ladder = ladder;
        _ladderCollider = _ladder.GetComponent<Collider>();
    }

    public override void OnExit()
    {
        _characterController.CurrentVelocity = new Vector3(_ladder.JumpOffForce.x, 0, _ladder.JumpOffForce.z);
        _characterController.VerticalVelocity = _ladder.JumpOffForce.y;

        _characterController.VerticalMove = true;
        _ladder = null;
    }

    public bool IsOnLadder()
    {
        if (_ladder == null)
            return false;

        float ladderBottom = _ladderCollider.bounds.min.y;
        float ladderTop = _ladderCollider.bounds.max.y;

        return _playerLadderTrigger.position.y < ladderTop && _playerLadderTrigger.position.y > ladderBottom;
    }

    protected override void Move(Vector3 wishDirection, float acceleration, float maxSpeed)
    {
        wishDirection.z = Mathf.Round(wishDirection.z);

        Vector3 moveDirection = _ladder.transform.up * _speed * wishDirection.z;

        _characterController.Move(moveDirection * Time.fixedDeltaTime);

        _characterController.Rotate(_ladder.transform.forward * -1);
    }
}
