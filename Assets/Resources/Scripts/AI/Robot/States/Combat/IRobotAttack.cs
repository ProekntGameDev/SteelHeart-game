using UnityEngine.AI;

namespace AI
{
    public interface IRobotAttack : IState
    {
        RobotAttackProperties AttackProperties { get; }

        void Init(NavMeshAgent navMeshAgent, Health health);

        bool IsDone();
    }
}
