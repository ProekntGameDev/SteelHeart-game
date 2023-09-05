using UnityEngine;
using NaughtyAttributes;
using Zenject;

[RequireComponent(typeof(Animator))]
public class PlayerIKLadder : MonoBehaviour
{
    [Header("IK Ladder")]
    [SerializeField] private float _ikSmoothTime;
    [SerializeField] private float _ikMaxSpeed;
    [SerializeField, Required] private Transform _ladderTrigger;

    [SerializeField] private Vector3 _rightHandOffest;
    [SerializeField] private Vector3 _rightHandRotation;
    [SerializeField] private Vector3 _leftHandOffest;
    [SerializeField] private Vector3 _leftHandRotation;
    [SerializeField] private Vector3 _rightFootOffest;
    [SerializeField] private Vector3 _leftFootOffest;

    [Inject] private Player _player;

    private bool _isRightSideUpdate;
    private Vector3? _lastLadderPoint;
    private Vector3 _targetPointRight;
    private Vector3 _targetPointLeft;
    private Vector3 _rightSmoothVelocity;
    private Vector3 _leftSmoothVelocity;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (_player.Movement.Ladder == null)
        {
            _lastLadderPoint = null;
            return;
        }

        Ladder ladder = _player.Movement.Ladder;

        if (_lastLadderPoint == null)
        {
            UpdateIK(ladder);

            _targetPointRight = _lastLadderPoint.Value;
            _targetPointLeft = _lastLadderPoint.Value;

            UpdateIKTransform(_lastLadderPoint.Value, _lastLadderPoint.Value, ladder);
            return;
        }

        UpdateIK(ladder);


        UpdateIKTransform(_targetPointRight, _targetPointLeft, ladder);
    }

    private void UpdateIK(Ladder ladder)
    {
        if (_lastLadderPoint != ladder.GetClosestLadder(_ladderTrigger.position))
        {
            _lastLadderPoint = ladder.GetClosestLadder(_ladderTrigger.position);
            _isRightSideUpdate = !_isRightSideUpdate;
        }

        _targetPointRight = Vector3.SmoothDamp(_targetPointRight, _lastLadderPoint.Value - new Vector3(0, _isRightSideUpdate ? 0 : .5f, 0), ref _rightSmoothVelocity, _ikSmoothTime, _ikMaxSpeed);
        _targetPointLeft = Vector3.SmoothDamp(_targetPointLeft, _lastLadderPoint.Value - new Vector3(0, _isRightSideUpdate ? .5f : 0, 0), ref _leftSmoothVelocity, _ikSmoothTime, _ikMaxSpeed);
    }

    private void UpdateIKTransform(Vector3 rightPoint, Vector3 leftPoint, Ladder ladder)
    {
        SetIKTransform(AvatarIKGoal.RightHand, rightPoint + ladder.transform.TransformDirection(_rightHandOffest), _rightHandRotation);
        SetIKTransform(AvatarIKGoal.RightFoot, rightPoint + ladder.transform.TransformDirection(_rightFootOffest));
        SetIKTransform(AvatarIKGoal.LeftHand, leftPoint + ladder.transform.TransformDirection(_leftHandOffest), _leftHandRotation);
        SetIKTransform(AvatarIKGoal.LeftFoot, leftPoint + ladder.transform.TransformDirection(_leftFootOffest));
    }

    private void SetIKTransform(AvatarIKGoal goal, Vector3 goalPosition, Vector3 goalLocalRotation = new Vector3())
    {
        _animator.SetIKPositionWeight(goal, 1);
        _animator.SetIKRotationWeight(goal, 1);

        _animator.SetIKPosition(goal, goalPosition);
        _animator.SetIKRotation(goal, Quaternion.Euler(transform.rotation.eulerAngles + goalLocalRotation));
    }
}
