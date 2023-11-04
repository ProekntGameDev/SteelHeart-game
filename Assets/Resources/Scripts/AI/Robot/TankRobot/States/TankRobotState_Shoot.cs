using UnityEngine;
using Zenject;

namespace AI
{
    public class TankRobotState_Shoot : MonoBehaviour, IState
    {
        [SerializeField] private AIMoveAgent _aiMoveAgent;
        [SerializeField] private TankAttackProperties _attackProperties;
        [SerializeField] private float _maxDistance;
        [SerializeField] private Health _health;

        [Inject] private Player _player;

        private float _endTime;

        public void OnEnter()
        {
            _endTime = Time.time + (1 / _attackProperties.Speed);
            _aiMoveAgent.UpdateRotation = false;
        }

        public void OnExit()
        {
            _aiMoveAgent.UpdateRotation = true;

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

        public bool IsDone()
        {
            return _player.Health.Current == 0 || _endTime <= Time.time;
        }

        public bool IsLostPlayer()
        {
            return Vector3.Distance(_player.transform.position, _aiMoveAgent.transform.position) > _maxDistance;
        }

        private void LookAtTarget()
        {
            Vector3 lookPosition = _player.transform.position - _aiMoveAgent.transform.position;
            lookPosition.y = 0f;
            Quaternion rotation = Quaternion.LookRotation(lookPosition);

            _aiMoveAgent.transform.rotation = Quaternion.RotateTowards(_aiMoveAgent.transform.rotation, rotation, _attackProperties.AimSpeed * Time.deltaTime);
        }

        private void Shoot(Vector3 direction)
        {
            Projectile projectile = Instantiate(_attackProperties.Projectile, _aiMoveAgent.transform);
            projectile.transform.forward = direction;
            projectile.transform.position = _attackProperties.ShootPoint.position;

            projectile.Init(direction, _attackProperties.Damage, _health);
        }
    }
}