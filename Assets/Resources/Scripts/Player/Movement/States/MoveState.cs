using UnityEngine;

namespace OldMovement
{
    public class MoveState : IState
    {
        [System.Serializable]
        public struct Settings
        {
            public float MaxSpeed => _maxSpeed;
            public float Acceleration => _acceleration;
            public float StaminRestoration => _staminRestoration;
            public float StaminCost => _staminCost;

            [SerializeField] private float _maxSpeed;
            [SerializeField] private float _acceleration;
            [SerializeField] private float _staminRestoration;
            [SerializeField] private float _staminCost;
        }

        public float StaminaCost => _settings.StaminCost;

        protected InertialCharacterController _characterController { get; private set; }

        private Settings _settings;
        private Stamina _stamina;

        public MoveState(InertialCharacterController characterController, Stamina stamina, Settings settings)
        {
            _characterController = characterController;
            _settings = settings;
            _stamina = stamina;
        }

        public virtual void OnEnter()
        {
            if (_stamina.Current < _settings.StaminCost)
                throw new System.InvalidOperationException();

            _stamina.Decay(_settings.StaminCost);
        }

        public virtual void OnExit()
        {
        }

        public void Tick()
        {
            Vector3 input = _characterController.ReadInputAxis();

            Vector3 forward = _characterController.Forward.normalized;
            Vector3 right = Vector3.Cross(Vector3.up, forward);

            Vector3 wishDirection = input.x * right + input.y * forward;

            wishDirection.Normalize();

            Move(wishDirection, _settings.Acceleration, _settings.MaxSpeed);

            if (_settings.StaminRestoration >= 0)
                _stamina.RestoreFixedTime(_settings.StaminRestoration);
            else
                _stamina.Decay(-_settings.StaminRestoration * Time.fixedDeltaTime);
        }

        protected virtual void Move(Vector3 wishDirection, float acceleration, float maxSpeed)
        {
            _characterController.GroundMove(wishDirection, acceleration, maxSpeed);
            _characterController.Rotate(wishDirection);
        }
    }
}
