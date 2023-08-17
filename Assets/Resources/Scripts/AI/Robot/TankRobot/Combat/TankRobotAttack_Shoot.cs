using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

namespace AI
{
    [CreateAssetMenu(menuName = "Scriptable Objects/TankRobot/Attacks/Shoot Attack")]
    public class TankRobotAttack_Shoot : ScriptableObject, ITankRobotAttack
    {
        public RobotAttackProperties AttackProperties => _attackProperties;

        [SerializeField] private RobotAttackProperties _attackProperties;

        private Player _player;
        private NavMeshAgent _navMeshAgent;

        private float _endTime;

        public void Init(NavMeshAgent navMeshAgent, Player player, Health robotHealth)
        {
            _player = player;
            _navMeshAgent = navMeshAgent;
        }

        
        // public void OnEnter()
        // {
        //     _endTime = Time.time + (1 / _attackProperties.Speed);
        // }
        //
        // public void OnExit()
        // {
        //     if (Vector3.Distance(_navMeshAgent.transform.position, _player.transform.position) < _attackProperties.MaxDistance)
        //         _player.Health.TakeDamage(_attackProperties.Damage);
        //
        //     _endTime = 0;
        // }
        //
        // public void Tick()
        // { }

        public bool IsDone() => _endTime <= Time.time || _player.Health.Current == 0;
    }
}
