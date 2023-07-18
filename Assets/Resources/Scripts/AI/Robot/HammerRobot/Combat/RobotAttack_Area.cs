using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Robot/Attacks/AOE Attack")]
    public class RobotAttack_Area : ScriptableObject, IRobotAttack
    {
        public RobotAttackProperties AttackProperties => _attackProperties;

        [SerializeField] private RobotAttackProperties _attackProperties;

        private NavMeshAgent _navMeshAgent;
        private Health _playerHealth;

        private float _endTime;

        public void Init(NavMeshAgent navMeshAgent, Health playerHealth)
        {
            _navMeshAgent = navMeshAgent;
            _playerHealth = playerHealth;
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

        public bool IsDone() => _endTime <= Time.time || _playerHealth.Current == 0;
    }
}
