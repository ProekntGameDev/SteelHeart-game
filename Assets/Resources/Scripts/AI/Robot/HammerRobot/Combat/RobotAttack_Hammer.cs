using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class RobotAttack_Hammer : IState
    {
        public Properties AttackProperties => _properties;

        private Properties _properties;
        private Player _player;
        private NavMeshAgent _navMeshAgent;

        private float _startTime;
        private bool _willAttack;

        public RobotAttack_Hammer(NavMeshAgent navMeshAgent, Player player, Properties properties)
        {
            _properties = properties;
            _player = player;
            _navMeshAgent = navMeshAgent;
        }

        public void OnEnter()
        {
            _navMeshAgent.updateRotation = false;
            _startTime = Time.time;
            _willAttack = true;
        }

        public void OnExit()
        {
            _startTime = 0;
        }

        public void Tick()
        {
            if (_willAttack == false)
                return;

            if (_startTime + _properties.PunchTime <= Time.time)
                Attack();

            _navMeshAgent.destination = _player.transform.position;

            Vector3 directionToPlayer = (_player.transform.position - _navMeshAgent.transform.position).normalized;
            _navMeshAgent.transform.forward = Vector3.Slerp(_navMeshAgent.transform.forward, directionToPlayer, _properties.RotationSlerp);
        }

        public bool IsDone() => _startTime + _properties.PunchTime + _properties.Delay <= Time.time || _player.Health.Current == 0;

        private void Attack()
        {
            if (Vector3.Distance(_navMeshAgent.transform.position, _player.transform.position) < _properties.MaxDistance)
                _player.Health.TakeDamage(_properties.Damage);

            _willAttack = false;
        }

        [System.Serializable]
        public class Properties
        {
            public float RotationSlerp => _rotationSlerp;
            public float Damage => _damage;
            public float MaxDistance => _maxDistance;
            public float PunchTime => _punchTime;
            public float Delay => _delay;

            [SerializeField, Range(0, 1)] private float _rotationSlerp;
            [SerializeField] private float _maxDistance;
            [SerializeField] private float _damage;
            [SerializeField] private float _punchTime;
            [SerializeField] private float _delay;
        }
    }
}
