using UnityEngine;

namespace AI
{
    [CreateAssetMenu(menuName = "Scriptable Objects/TankRobot/Attacks/Shoot Attack")]
    public class TankRobotAttack_Shoot : ScriptableObject, ITankRobotAttack
    {
        public TankRobotAttackProperties AttackProperties => _attackProperties;

        [SerializeField] private TankRobotAttackProperties _attackProperties;

    }
}
