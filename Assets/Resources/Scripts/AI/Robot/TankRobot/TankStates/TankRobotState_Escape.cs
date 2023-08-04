using UnityEngine.AI;
using UnityEngine;

namespace AI
{
    public class TankRobotState_Escape : IState
    {
        private float _speed;
        private float _escapingDistance;
        private float _stoppingDistance;
        private NavMeshAgent _navMeshAgent;
        private Player _player;

        public TankRobotState_Escape(float speed, float escapingDistance, float stoppingDistance, NavMeshAgent navMeshAgent, Player player)
        {
            _speed = speed;
            _escapingDistance = escapingDistance;
            _stoppingDistance = stoppingDistance;
            _navMeshAgent = navMeshAgent;
            _player = player;
        }
        public void OnEnter()
        {
            _navMeshAgent.stoppingDistance = _stoppingDistance;
            _navMeshAgent.speed = _speed;
        }

        public void OnExit()
        { }
        
        public void Tick()
        {
            Vector3 directionToPlayer = _navMeshAgent.transform.position - _player.transform.position;
            Vector3 newPosition = _navMeshAgent.transform.position + directionToPlayer;

            _navMeshAgent.SetDestination(newPosition);
        }

        public bool IsPlayerClose()
        {
            return Vector3.Distance(_player.transform.position, _navMeshAgent.transform.position) < _escapingDistance;
        }
        
        public bool IsPlayerFar()
        {
            return Vector3.Distance(_player.transform.position, _navMeshAgent.transform.position) > _escapingDistance * 2;
        }
    }
}