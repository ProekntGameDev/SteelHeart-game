using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Robot/Attacks/Hammer Attack")]
    public class RobotAttack_Hammer : ScriptableObject, IRobotAttack
    {
        public RobotAttackProperties AttackProperties => _attackProperties;

        [SerializeField] private RobotAttackProperties _attackProperties;

        private Health _playerHealth;
        private NavMeshAgent _navMeshAgent;

        private float _endTime;

        public void Init(NavMeshAgent navMeshAgent, Health playerHealth)
        {
            _playerHealth = playerHealth;
            _navMeshAgent = navMeshAgent;
        }

        public void OnEnter()
        {
            _endTime = Time.time + (1 / _attackProperties.Speed);
        }

        public void OnExit()
        {
            if (Vector3.Distance(_navMeshAgent.transform.position, _playerHealth.transform.position) < _attackProperties.MaxDistance)
                _playerHealth.TakeDamage(_attackProperties.Damage);

            _endTime = 0;
        }

        public void Tick()
        { }

        public bool IsDone() => _endTime <= Time.time || _playerHealth.Current == 0;
    }
}
