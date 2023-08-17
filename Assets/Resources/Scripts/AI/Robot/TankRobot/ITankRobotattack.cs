using UnityEngine.AI;

namespace AI
{
    public interface ITankRobotAttack
    {
        RobotAttackProperties AttackProperties { get; }

        void Init(NavMeshAgent navMeshAgent, Player player, Health robotHealth);

        bool IsDone();
    }
}