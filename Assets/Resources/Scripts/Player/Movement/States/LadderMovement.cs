using UnityEngine;

public class LadderMovement : BaseMovementState
{
    public override bool CanUseCombat() => false;

    public Ladder Ladder { get; private set; }

    [SerializeField] private Transform _playerLadderTrigger;
    [SerializeField] private float _speed;

    private Collider _ladderCollider;

    public void Init(Ladder ladder)
    {
        Ladder = ladder;
        _ladderCollider = Ladder.GetComponent<Collider>();
    }

    public void ResetLadder()
    {
        Ladder = null;
        _ladderCollider = null;
    }

    public override void Enter()
    {
        CharacterController.CurrentVelocity = Vector3.zero;
        CharacterController.VerticalVelocity = 0;

        CharacterController.VerticalMove = false;
    }

    public override void Tick()
    {
        if (IsClimbOnLadder())
        {
            ClimbOnLadder();
            return;
        }

        Vector3 wishDirection = GetWishDirection(CharacterController);

        float input = Mathf.Round(wishDirection.z);

        Vector3 moveDirection = Ladder.transform.up * _speed * input;
        moveDirection += Ladder.GetCenterOffset(CharacterController.transform.position);

        CharacterController.Move(moveDirection * Time.fixedDeltaTime);

        CharacterController.Rotate(Ladder.transform.forward * -1);
    }

    public override void Exit()
    {
        if (_playerLadderTrigger.position.y < _ladderCollider.bounds.max.y + CharacterController.Height / 2)
        {
            CharacterController.CurrentVelocity = new Vector3(Ladder.JumpOffForce.x, 0, Ladder.JumpOffForce.z);
            CharacterController.VerticalVelocity = Ladder.JumpOffForce.y;
        }
        else
            CharacterController.CurrentVelocity = Ladder.transform.forward * _speed * -1;

        CharacterController.VerticalMove = true;

        ResetLadder();
    }

    public bool IsOnLadder()
    {
        if (Ladder == null)
            return false;

        float ladderBottom = _ladderCollider.bounds.min.y;
        float ladderTop = _ladderCollider.bounds.max.y + CharacterController.Height / 2;

        return _playerLadderTrigger.position.y < ladderTop && _playerLadderTrigger.position.y > ladderBottom;
    }

    private void ClimbOnLadder()
    {
        Vector3 moveDirection = (Ladder.transform.up + Ladder.transform.forward * -1) * _speed;

        CharacterController.Move(moveDirection * Time.fixedDeltaTime);

        CharacterController.Rotate(Ladder.transform.forward * -1);
    }

    private bool IsClimbOnLadder()
    {
        if (Ladder == null)
            return false;

        float ladderTop = _ladderCollider.bounds.max.y;

        return CharacterController.transform.position.y + CharacterController.Height > ladderTop;
    }
}
