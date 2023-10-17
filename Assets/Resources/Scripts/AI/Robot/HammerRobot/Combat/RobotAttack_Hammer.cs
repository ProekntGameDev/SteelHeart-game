using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace AI
{
    public class RobotAttack_Hammer : MonoBehaviour, IState
    {
        public Properties AttackProperties => _properties;

        [SerializeField] private Properties _properties;
        [SerializeField, Required] private Health _robotHealth;
        [SerializeField, Required] private NavMeshAgent _navMeshAgent;

        [SerializeField, Required] private Animator _animator;
        [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Trigger)] private string _trigger;

        [Inject] private Player _player;

        private float _startTime;
        private bool _willAttack;

        public void OnEnter()
        {
            _navMeshAgent.updateRotation = false;
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
                _player.Health.TakeDamage(_properties.Damage, _robotHealth);

            _willAttack = false;
        }

        [System.Serializable]
        public class Properties
        {
            public float RotationSlerp => _rotationSlerp;
            public Damage Damage => _damage;
            public float MaxDistance => _maxDistance;
            public float PunchTime => _punchTime;
            public float Delay => _delay;

            [SerializeField, Range(0, 1)] private float _rotationSlerp;
            [SerializeField] private float _maxDistance;
            [SerializeField] private Damage _damage;
            [SerializeField] private float _punchTime;
            [SerializeField] private float _delay;
        }
    }
}
