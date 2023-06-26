using UnityEngine;

namespace NewPlayerController
{
    public class CheckIsGrounded : MonoBehaviour
    {
        public bool IsGrounded { get; set; }

        [Header("CheckIsGraunded")]
        [SerializeField] private Transform _groundRay;
        [SerializeField] private float _groundRayLength = 0.2f;
        [SerializeField] private CharacterController _characterController;

        private float _yVelocity;
        private const float Gravity = -9.81f;

        private void Update()
        {
            ApplyGravity();
            IsGrounded &= CheckGrounded();
        }

        private bool CheckGrounded()
        {
            if (Physics.Raycast(new Ray(_groundRay.position, Vector3.down), out RaycastHit raycastHit, _groundRayLength))
                if (Vector3.Angle(Vector3.up, raycastHit.normal) <= _characterController.slopeLimit)
                    if (_yVelocity < _groundRayLength)
                        return true;

            return false;
        }

        private void ApplyGravity()
        {
            if (IsGrounded == false)
            {
                _yVelocity = Mathf.Max(Gravity * Time.deltaTime + _yVelocity, Gravity * Time.deltaTime);
                _yVelocity = Mathf.Max(Gravity, _yVelocity);
            }
            else
                _yVelocity = Mathf.Max(Gravity * Time.deltaTime * Time.deltaTime, _yVelocity - Gravity * Time.deltaTime * Time.deltaTime);

        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (Vector3.Angle(Vector3.up, hit.normal) > _characterController.slopeLimit)
                return;

            IsGrounded = true;
            _yVelocity = Gravity * Time.deltaTime * Time.deltaTime;
        }
    }
}

