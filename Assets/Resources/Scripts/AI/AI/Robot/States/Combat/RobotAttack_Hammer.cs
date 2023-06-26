using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class RobotAttack_Hammer : IRobotAttack
    {
        public RobotAttackProperties AttackProperties => _attackProperties;

        private Health _health;
        private NavMeshAgent _navMeshAgent;
        private RobotAttackProperties _attackProperties;

        private float _endTime;

        public RobotAttack_Hammer(Health health, NavMeshAgent navMeshAgent, RobotAttackProperties robotAttackProperties)
        {
            _health = health;
            _navMeshAgent = navMeshAgent;
            _attackProperties = robotAttackProperties;
        }

        public void OnEnter()
        {
            _endTime = Time.time + (1 / _attackProperties.Speed);
        }

        public void OnExit()
        {
            if (Vector3.Distance(_navMeshAgent.transform.position, _health.transform.position) < _attackProperties.MaxDistance)
                _health.TakeDamage(_attackProperties.Damage);

            _endTime = 0;
        }

        public void Tick()
        { }

        public bool IsDone() => _endTime <= Time.time;
    }
}
