using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class TankRobotState_Shoot : IState
    {
        private NavMeshAgent _navMeshAgent;
        private ITankRobotAttack _attack;

        private float _maxDistance;
        private Player _player;


        private float _endTime;
        private const float InterpolationRatio = 0.3f;
        public TankRobotState_Shoot(Player player, NavMeshAgent navMeshAgent, float maxDistance, ITankRobotAttack attack)
        {
            _navMeshAgent = navMeshAgent;
            _player = player;
            _maxDistance = maxDistance;
            _attack = attack;
        }

        public void OnEnter()
        {
            _endTime = Time.time + (1 / _attack.AttackProperties.Speed);
        }

        public void OnExit()
        {
            if (Vector3.Distance(_navMeshAgent.transform.position, _player.transform.position) < _attack.AttackProperties.MaxDistance)
                _player.Health.TakeDamage(_attack.AttackProperties.Damage);
            
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