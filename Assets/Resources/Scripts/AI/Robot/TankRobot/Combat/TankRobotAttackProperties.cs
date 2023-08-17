using UnityEngine;

namespace AI
{
    [System.Serializable]
    public class TankRobotAttackProperties
    {
        public float MinDistance => _minDistance;
        public float MaxDistance => _maxDistance;
        public float Damage => _damage;
        public float Speed => _speed;

        [SerializeField] private float _minDistance;
        [SerializeField] private float _maxDistance;
        [SerializeField] private float _damage;
        [SerializeField] private float _speed;
    }
}