using Features.Buttons;
using UnityEngine;

namespace Features.Lift
{
    [RequireComponent(typeof(Animator))]
    public class Lift : MonoBehaviour
    {
        private enum StartPosition { Down, Up }

        [SerializeField] private float _speed = 1f;
        [SerializeField] private float _minOpenedTime = 2f;

        [SerializeField] private StartPosition _startPosition = StartPosition.Down;

        [SerializeField] private AbstractActivatedSource _callToDownButton;
        [SerializeField] private AbstractActivatedSource _callToUpButton;
        [SerializeField] private AbstractActivatedSource _moveButton;

        [SerializeField] private Transform _upPoint;
        [SerializeField] private Transform _downPoint;
        [SerializeField] private Transform _platform;

        private Animator _animator;

        private BaseHierarchicalState _currentState;
        private StatesFactory _states;

        public BaseHierarchicalState CurrentState
        {
            get => _currentState;
            set => _currentState = value;
        }

        public bool HasCallToUp { get; set; }
        public bool HasCallToDown { get; set; }

        public float Speed => _speed;
        public float MinOpenedTime => _minOpenedTime;

        private void Awake()
        {
            if (_startPosition == StartPosition.Down)
                _platform.position = _downPoint.position;
            else
                _platform.position = _upPoint.position;

            _animator = GetComponent<Animator>();

            _states = new StatesFactory(this, _upPoint, _downPoint, _platform, _animator);
            _currentState = _states.GetSate<ClosedDoorState>();
            _currentState.EnterState();

            if (_callToUpButton != null)
                _callToUpButton.Activated += OnCallToUp;

            if (_callToDownButton != null)
                _callToDownButton.Activated += OnCallToDown;

            if (_moveButton != null)
                _moveButton.Activated += OnCallMove;
        }

        private void OnCallMove()
        {
            if (DownLevel)
                HasCallToUp = true;

            if (UpLevel)
                HasCallToDown = true;

            _currentState.Call();
        }

        private void OnCallToDown()
        {
            HasCallToDown = true;
            _currentState.Call();
        }

        private void OnCallToUp()
        {
            HasCallToUp = true;
            _currentState.Call();
        }

        private void Update()
        {
            _currentState.UpdateState();
        }

        private void FixedUpdate()
        {
            _currentState.FixedUpdateState();
        }

        private void OnAnimationEvent(string param)
        {
            _currentState.GetAnimationEvent(param);
        }

        public bool DownLevel
        {
            get
            {
                float distanceToDownPoint = Vector3.Distance(_platform.position, _downPoint.position);

                if (distanceToDownPoint < Mathf.Epsilon)
                    return true;

                return false;
            }
        }

        public bool UpLevel
        {
            get
            {
                float distanceToUpPoint = Vector3.Distance(_platform.position, _upPoint.position);

                if (distanceToUpPoint < Mathf.Epsilon)
                    return true;

                return false;
            }
        }
    }
}
