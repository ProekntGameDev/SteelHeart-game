using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace QTE
{
    public class QTETrainPush : QTEBehavior
    {
        [Header("Player")]
        [SerializeField, Required] private Animator _playerAnimator;
        [SerializeField, AnimatorLayer(nameof(_playerAnimator))] private int _qteLayer;
        [SerializeField, AnimatorState(nameof(_playerAnimator), nameof(_qteLayer))] private string _pushState;
        [SerializeField, AnimatorParam(nameof(_playerAnimator), AnimatorControllerParameterType.Float)] private string _pushSpeed;
        [SerializeField, AnimatorParam(nameof(_playerAnimator), AnimatorControllerParameterType.Trigger)] private string _onEndQte;

        [Header("Train")]
        [SerializeField, Required] private Transform _playerStartPoint;
        [SerializeField, Required] private Transform _target;
        [SerializeField, Required] private Transform _startPoint;
        [SerializeField, Required] private Transform _endPoint;
        [SerializeField] private float _smoothTime;

        [Inject] private Player _player;

        private Vector3 _currentVelocity;
        private Vector3 _targetPosition;

        private bool _isQteActive;
        private bool _isDone = true;

        public override void OnStart()
        {
            _player.Movement.enabled = false;
            _player.transform.position = _playerStartPoint.position;

            _isQteActive = true;
            _isDone = false;

            _playerAnimator.Play(_pushState);
        }

        public override void OnProgressChanged(float progress)
        {
            _targetPosition = Vector3.Lerp(_startPoint.position, _endPoint.position, progress);

            float pushSpeed = 0;

            if (Vector3.Dot((_endPoint.position - _startPoint.position).normalized, _currentVelocity.normalized) == 1) // If vectors pointing in exactly same direction
                pushSpeed = _currentVelocity.magnitude;

            _playerAnimator.SetFloat(_pushSpeed, pushSpeed);
        }

        public override void OnEnd(bool result)
        {
            _targetPosition = result ? _endPoint.position : _startPoint.position;

            _isQteActive = false;
        }

        private void Update()
        {
            if (_isDone)
                return;

            _target.transform.position = Vector3.SmoothDamp(_target.transform.position, 
                _targetPosition, ref _currentVelocity, _smoothTime);

            _player.transform.position = _playerStartPoint.position + (_target.position - _startPoint.position);

            Quaternion lookDirection = Quaternion.LookRotation((_endPoint.position - _startPoint.position).normalized);

            _player.transform.rotation = lookDirection;
            _target.transform.rotation = lookDirection;

            if (_isQteActive)
                return;

            if (Vector3.Distance(_player.transform.position, _playerStartPoint.position) < 0.1f || HasPlayerReachedEndPoint())
            {
                _player.Movement.enabled = true;

                _isDone = true;
                _playerAnimator.SetTrigger(_onEndQte);
            }
        }

        private bool HasPlayerReachedEndPoint() => Vector3.Distance(_player.transform.position, _playerStartPoint.position + (_endPoint.position - _startPoint.position)) < 0.1f;
    }
}
