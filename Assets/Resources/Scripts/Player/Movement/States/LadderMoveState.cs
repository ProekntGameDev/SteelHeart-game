using UnityEngine;

public class LadderMoveState : OldMovement.MoveState
{
    public Ladder Ladder => _ladder;

    private Transform _playerLadderTrigger;
    private Ladder _ladder;
    private Collider _ladderCollider;
    private float _speed;

    public LadderMoveState(InertialCharacterController characterController, Stamina stamina, Transform playerLadderTrigger, float speed) 
        : base(characterController, stamina, new Settings())
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

    public void SetLadder(Ladder ladder)
    {
        _ladder = ladder;
        _ladderCollider = _ladder.GetComponent<Collider>();
    }

    public void ResetLadder()
    {
        _ladder = null;
        _ladderCollider = null;
    }

    public override void OnExit()
    {
        if (_playerLadderTrigger.position.y < _ladderCollider.bounds.max.y + _characterController.Height / 2)
        {
            _characterController.CurrentVelocity = new Vector3(_ladder.JumpOffForce.x, 0, _ladder.JumpOffForce.z);
            _characterController.VerticalVelocity = _ladder.JumpOffForce.y;
        }
        else
            _characterController.CurrentVelocity = _ladder.transform.forward * _speed * -1;

        _characterController.VerticalMove = true;

        ResetLadder();
    }

    public bool IsOnLadder()
    {
        if (_ladder == null)
            return false;

        float ladderBottom = _ladderCollider.bounds.min.y;
        float ladderTop = _ladderCollider.bounds.max.y + _characterController.Height / 2;

        return _playerLadderTrigger.position.y < ladderTop && _playerLadderTrigger.position.y > ladderBottom;
    }

    public bool IsClimbOnLadder()
    {
        if (_ladder == null)
            return false;

        float ladderTop = _ladderCollider.bounds.max.y;

        return _characterController.transform.position.y + _characterController.Height > ladderTop;
    }

    protected override void Move(Vector3 wishDirection, float acceleration, float maxSpeed)
    {
        if (IsClimbOnLadder())
        {
            ClimbOnLadder();
            return;
        }

        wishDirection.z = Mathf.Round(wishDirection.z);

        Vector3 moveDirection = _ladder.transform.up * _speed * wishDirection.z;
        moveDirection += _ladder.GetCenterOffset(_characterController.transform.position);

        _characterController.Move(moveDirection * Time.fixedDeltaTime);

        _characterController.Rotate(_ladder.transform.forward * -1);
    }

    private void ClimbOnLadder()
    {
        Vector3 moveDirection = (_ladder.transform.up + _ladder.transform.forward * -1) * _speed;

        _characterController.Move(moveDirection * Time.fixedDeltaTime);

        _characterController.Rotate(_ladder.transform.forward * -1);
    }
}
