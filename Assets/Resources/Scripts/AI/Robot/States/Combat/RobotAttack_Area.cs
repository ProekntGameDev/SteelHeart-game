using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class RobotAttack_Area : IRobotAttack
    {
        public RobotAttackProperties AttackProperties => _attackProperties;

        private NavMeshAgent _navMeshAgent;
        private RobotAttackProperties _attackProperties;

        private float _endTime;

        public RobotAttack_Area(NavMeshAgent navMeshAgent, RobotAttackProperties robotAttackProperties)
        {
            _navMeshAgent = navMeshAgent;
            _attackProperties = robotAttackProperties;
        }

        public void OnEnter()
        {
            _endTime = Time.time + (1 / _attackProperties.Speed);
        }

        public void OnExit()
        {
            Collider[] colliders = Physics.OverlapSphere(_navMeshAgent.transform.position, _attackProperties.MaxDistance);

            foreach (var collider in colliders)
                if (collider.TryGetComponent(out IDamagable damagable))
                    damagable.TakeDamage(_attackProperties.Damage);

            _endTime = 0;
        }

        public void Tick()
        { }

        public bool IsDone() => _endTime <= Time.time;
    }
}
