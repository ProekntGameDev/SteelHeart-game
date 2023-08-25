using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class TankRobotState_Shoot : IState
    {
        private NavMeshAgent _navMeshAgent;
        private AttackProperties _attackProperties;

        private float _maxDistance;
        private Player _player;

        private float _endTime;

        public TankRobotState_Shoot
            (Player player, NavMeshAgent navMeshAgent, float maxDistance, AttackProperties attackProperties)
        {
            _navMeshAgent = navMeshAgent;
            _player = player;
            _maxDistance = maxDistance;
            _attackProperties = attackProperties;
        }

        public void OnEnter()
        {
            _endTime = Time.time + (1 / _attackProperties.Speed);
            _navMeshAgent.updateRotation = false;
        }

        public void OnExit()
        {
            _navMeshAgent.updateRotation = true;

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
            return Vector3.Distance(_player.transform.position, _navMeshAgent.transform.position) > _maxDistance;
        }

        private void LookAtTarget()
        {
            Vector3 lookPosition = _player.transform.position - _navMeshAgent.transform.position;
            lookPosition.y = 0f;
            Quaternion rotation = Quaternion.LookRotation(lookPosition);

            _navMeshAgent.transform.rotation = Quaternion.RotateTowards(_navMeshAgent.transform.rotation, rotation, _attackProperties.AimSpeed * Time.deltaTime);
        }

        private void Shoot(Vector3 direction)
        {
            Projectile projectile = GameObject.Instantiate(_attackProperties.Projectile, _navMeshAgent.transform);
            projectile.transform.forward = direction;
            projectile.transform.position = _attackProperties.ShootPoint.position;

            projectile.Init(direction, _attackProperties.Damage);
        }
    }
}