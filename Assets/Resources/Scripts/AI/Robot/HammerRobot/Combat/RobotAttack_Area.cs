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
        private Player _player;
        private Health _robotHealth;

        private float _endTime;

        public void Init(NavMeshAgent navMeshAgent, Player player, Health robotHealth)
        {
            _navMeshAgent = navMeshAgent;
            _player = player;
            _robotHealth = robotHealth;
        }

        public void OnEnter()
        {
            _endTime = Time.time + (1 / _attackProperties.Speed);
        }

        public void OnExit()
        {
            if (IsDone())
            {
                Collider[] colliders = Physics.OverlapSphere(_navMeshAgent.transform.position, _attackProperties.MaxDistance);

                foreach (var collider in colliders)
                    if (collider.TryGetComponent(out IDamagable damagable))
                        if (collider.TryGetComponent(out Health health) && health != _robotHealth)
                            damagable.TakeDamage(_attackProperties.Damage);
            }

            _endTime = 0;
        }

        public void Tick()
        { }

        public bool IsDone() => _endTime <= Time.time || _player.Health.Current == 0;
    }
}
