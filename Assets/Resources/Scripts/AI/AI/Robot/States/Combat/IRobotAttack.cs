using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public interface IRobotAttack : IState
    {
        RobotAttackProperties AttackProperties { get; }

        bool IsDone();
    }
}
