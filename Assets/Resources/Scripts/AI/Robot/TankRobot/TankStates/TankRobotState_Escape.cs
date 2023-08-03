using UnityEngine.AI;
using UnityEngine;

namespace AI
{
    public class TankRobotState_Escape : IState
    {
        private float _escapingDistance;
        private NavMeshAgent _navMeshAgent;
        private Player _player;

        public TankRobotState_Escape(float escapingDistance, NavMeshAgent navMeshAgent, Player player)
        {
            _escapingDistance = escapingDistance;
            _navMeshAgent = navMeshAgent;
            _player = player;
        }
        public void OnEnter()
        {
            
        }

        public void OnExit()
        {
            
        }
        
        public void Tick()
        {
            
        }
    }
}