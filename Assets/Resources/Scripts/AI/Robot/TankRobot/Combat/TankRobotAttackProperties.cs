using UnityEngine;

namespace AI
{
    [System.Serializable]
    public class TankRobotAttackProperties : MonoBehaviour
    {
        public float Damage => _damage;
        public float Speed => _speed;
        public float AttackDelay => _attackDelay;

        [SerializeField] private float _damage;
        [SerializeField] private float _speed;
        [SerializeField] private float _attackDelay;
    }
}

