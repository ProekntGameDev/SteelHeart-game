using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class DroneState_Attack : IState
    {
        private NavMeshAgent _navMeshAgent;
        private DroneAttackProperties _attackProperties;

        private float _minDistance;
        private float _maxDistance;
        private float _baseStoppingDistance;
        
        private Player _player;

        private float _endTime;

        public DroneState_Attack
            (Player player, NavMeshAgent navMeshAgent, float minDistance, float maxDistance, DroneAttackProperties attackProperties)
        {
            _navMeshAgent = navMeshAgent;
            _player = player;
            _minDistance = minDistance;
            _maxDistance = maxDistance;
            _attackProperties = attackProperties;
        }

        public void OnEnter()
        {
            _endTime = Time.time + (1 / _attackProperties.Speed);
            _navMeshAgent.updateRotation = false;

            _baseStoppingDistance = _navMeshAgent.stoppingDistance;
            _navMeshAgent.stoppingDistance = _minDistance;
        }

        public void OnExit()
        {
            _navMeshAgent.updateRotation = true;
            _navMeshAgent.stoppingDistance = _baseStoppingDistance;
            
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