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

        private float distance;
        private float _endTime;
        private const float InterpolationRatio = 0.3f;
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
        }

        public void OnExit()
        {
            distance = Vector3.Distance(_navMeshAgent.transform.position, _player.transform.position);
            if (distance < _attackProperties.MaxDistance && distance > _attackProperties.MinDistance)
                _player.Health.TakeDamage(_attackProperties.Damage);
            
            _endTime = 0;
        }

        public void Tick()
        {
            LookAtTarget();
        }

        public bool IsDone()
        {
            return _player.Health.Current == 0 || _endTime < Time.time;
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

            _navMeshAgent.transform.rotation = Quaternion.Slerp(_navMeshAgent.transform.rotation, rotation, InterpolationRatio);
        }
    }
}