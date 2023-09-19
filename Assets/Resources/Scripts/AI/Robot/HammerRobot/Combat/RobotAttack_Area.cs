using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class RobotAttack_Area : IState
    {
        public Properties AttackProperties => _properties;

        private Properties _properties;

        private NavMeshAgent _navMeshAgent;
        private Player _player;
        private Health _robotHealth;

        private float _startTime;
        private bool _willAttack;

        public RobotAttack_Area(NavMeshAgent navMeshAgent, Player player, Health robotHealth, Properties properties)
        {
            _navMeshAgent = navMeshAgent;
            _player = player;
            _robotHealth = robotHealth;
            _properties = properties;
        }

        public void OnEnter()
        {
            _navMeshAgent.updateRotation = true;
            _navMeshAgent.speed = _properties.AirSpeed;
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

            if (_startTime + _properties.JumpTime <= Time.time)
                Attack();

            _navMeshAgent.destination = _player.transform.position;
        }

        public bool IsDone() => _startTime + _properties.JumpTime + _properties.Delay <= Time.time || _player.Health.Current == 0;

        private void Attack()
        {
            Collider[] colliders = Physics.OverlapSphere(_navMeshAgent.transform.position, _properties.AoERadius);

            foreach (var collider in colliders)
                if (collider.TryGetComponent(out IDamagable damagable))
                    if (collider.TryGetComponent(out Health health) && health != _robotHealth)
                        damagable.TakeDamage(_properties.Damage);

            _willAttack = false;
        }

        [System.Serializable]
        public class Properties
        {
            public float JumpDistance => _jumpTime * _airSpeed;
            public float AirSpeed => _airSpeed;
            public float AoERadius => _aoeRadius;
            public float Damage => _damage;
            public float JumpTime => _jumpTime;
            public float Delay => _delay;

            [SerializeField] private float _airSpeed;
            [SerializeField] private float _aoeRadius;
            [SerializeField] private float _damage;
            [SerializeField] private float _jumpTime;
            [SerializeField] private float _delay;
        }
    }
}
