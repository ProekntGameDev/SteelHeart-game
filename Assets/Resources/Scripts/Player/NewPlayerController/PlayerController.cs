using UnityEngine;

namespace NewPlayerController
{
    public class PlayerController : MonoBehaviour, IPlayerBehaviourData, IControllable
    {
        //data for behaviors
        public CharacterController CharacterController => _characterController;
        public Transform TransformPlayer => _transformPlayer;
        public float X { get; private set; }
        public float Z { get; private set; }
        public PlayerSpeed SpeedPlayer { get; private set; } = new PlayerSpeed();
        public IPlayerAnimator PlayerAnimator { get; private set; }
        public PlayerMovement PlayerMovement => _playerMovement;
        public bool IsGrounded => _checkIsGrounded.IsGrounded;

        public IPlayerBehaviour PlayerBehaviour => _playerBehaviourController?.CurrentPlayerBehaviour; //current player behavior

        [Header("PlayerBehaviourSettings")]
        public float WalkSpeed = 3f;
        public float RunSpeed = 6f;
        public float HalfSquatSpeed = 2f;
        public float RiseSpeed = 3f;
        public float Gravity = 0f;
        public float JumpForce = 12f;

        [Header("PlayerData")]
        [SerializeField] private CharacterController _characterController; //player CharacterController
        [SerializeField] private Transform _transformPlayer; //player Transform
        [SerializeField] private PlayerMovement _playerMovement; //player movement

        [Header("CheckIsGraunded")]
        [SerializeField] private CheckIsGrounded _checkIsGrounded; //check is grounded

        [SerializeField] private string _currentBehaviour;

        private PlayerBehaviourController _playerBehaviourController; //managing player behaviors

        private void Awake()
        {
            _playerBehaviourController = new PlayerBehaviourController(this);
            PlayerAnimator = GetComponent<IPlayerAnimator>(); //receiving IPlayerAnimator
        }

        private void Update()
        {
            PlayerBehaviour.UpdateBehaviour();
            _currentBehaviour = $"{PlayerBehaviour.GetType()}";
        }

        public void Move(float x, float z, bool isShift)
        {
            X = x;
            Z = z;
            if (isShift) PlayerBehaviour.SetNewBehaviour<RunPlayerBehaviour>();
            else PlayerBehaviour.SetNewBehaviour<WalkPlayerBehaviour>();
        }

        public void Jump() => PlayerBehaviour.SetNewBehaviour<JumpPlayerBehaviour>();

        public void HalfSquat() => PlayerBehaviour.SetNewBehaviour<HalfSquatPlayerBehaviour>();

        public void RiseUp() => PlayerBehaviour.SetNewBehaviour<RisePlayerBehaviour>();
    }
}
