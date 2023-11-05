using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace AI
{
    public class DroneState_Attack : MonoBehaviour, IState
    {
        [HideInInspector] public UnityEvent OnShoot;

        [SerializeField, Required] private AIMoveAgent _aiMoveAgent;
        [SerializeField, Required] private Health _droneHealth;

        [SerializeField] private DroneAttackProperties _attackProperties;
        [SerializeField] private float _minDistance;
        [SerializeField] private float _maxDistance;

        [Inject] private Player _player;

        private float _endTime;
        private float _baseStoppingDistance;
        private int _attacksBeforeFreeze;

        public void OnEnter()
        {
            _endTime = Time.time + (1 / _attackProperties.Speed);
            
            _aiMoveAgent.UpdateRotation = false;

            _baseStoppingDistance = _aiMoveAgent.StoppingDistance;
            _aiMoveAgent.StoppingDistance = _minDistance;
        }

        public void OnExit()
        {
            _aiMoveAgent.UpdateRotation = true;
            _aiMoveAgent.StoppingDistance = _baseStoppingDistance;
            
            if (_endTime > Time.time)
                return;

            Vector3 playerPosition = _player.transform.position + (_player.Movement.CharacterController.Height / 2 * Vector3.up);
            Ray ray = new Ray(_attackProperties.ShootPoint.position, (playerPosition - _attackProperties.ShootPoint.position).normalized);

            if (Physics.Raycast(ray, out RaycastHit hit, _maxDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
                if (hit.collider.TryGetComponent(out Player player))
                    Shoot(ray.direction);
        }

        public void Tick()
        {
            LookAtTarget();
        }

        public void Reload()
        {
            _attacksBeforeFreeze = _attackProperties.AttacksBeforeFreeze;
        }

        public bool IsDone() =>_player.Health.Current == 0 || _endTime <= Time.time;

        public bool IsLostPlayer() => Vector3.Distance(_player.transform.position, _aiMoveAgent.transform.position) > _maxDistance;

        public bool IsTimeOut() => _attacksBeforeFreeze == 0;

        private void LookAtTarget()
        {
            Vector3 lookPosition = _player.transform.position - _aiMoveAgent.transform.position;
            lookPosition.y = 0f;
            Quaternion rotation = Quaternion.LookRotation(lookPosition);

            _aiMoveAgent.transform.rotation = Quaternion.RotateTowards(_aiMoveAgent.transform.rotation, rotation, _attackProperties.AimSpeed * Time.deltaTime);
        }

        private void Shoot(Vector3 direction)
        {
            OnShoot?.Invoke();

            Projectile projectile = Instantiate(_attackProperties.Projectile, _aiMoveAgent.transform);
            projectile.transform.forward = direction;
            projectile.transform.position = _attackProperties.ShootPoint.position;

            projectile.Init(direction, _attackProperties.Damage, _droneHealth);

            _attacksBeforeFreeze -= 1;
        }
    }
}