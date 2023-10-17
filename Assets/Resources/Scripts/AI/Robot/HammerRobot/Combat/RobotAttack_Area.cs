using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace AI
{
    public class RobotAttack_Area : MonoBehaviour, IState
    {
        public Properties AttackProperties => _properties;

        [SerializeField] private Properties _properties;
        [SerializeField, Required] private NavMeshAgent _navMeshAgent;
        [SerializeField, Required] private Health _robotHealth;

        [SerializeField, Required] private Animator _animator;
        [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Trigger)] private string _trigger;

        [Inject] private Player _player;

        private float _startTime;
        private bool _willAttack;

        public void OnEnter()
        {
            _navMeshAgent.updateRotation = true;
            _navMeshAgent.speed = _properties.AirSpeed;
            _startTime = Time.time;
            _willAttack = true;

            _animator.SetTrigger(_trigger);
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
                        damagable.TakeDamage(_properties.Damage, _robotHealth);

            _willAttack = false;
        }

        [System.Serializable]
        public class Properties
        {
            public float JumpDistance => _jumpTime * _airSpeed;
            public float AirSpeed => _airSpeed;
            public float AoERadius => _aoeRadius;
            public Damage Damage => _damage;
            public float JumpTime => _jumpTime;
            public float Delay => _delay;

            [SerializeField] private float _airSpeed;
            [SerializeField] private float _aoeRadius;
            [SerializeField] private Damage _damage;
            [SerializeField] private float _jumpTime;
            [SerializeField] private float _delay;
        }
    }
}
