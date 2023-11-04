using UnityEngine;
using NaughtyAttributes;
using Zenject;

namespace AI
{
    public class TankRobotState_Escape : MonoBehaviour, IState
    {
        private const float EscapeMultiplier = 2f;

        [SerializeField, Required] private AIMoveAgent _aiMoveAgent;
        [SerializeField] private float _speed;
        [SerializeField] private float _escapingDistance;
        [SerializeField] private float _stoppingDistance;

        [Inject] private Player _player;

        public void OnEnter()
        {
            _aiMoveAgent.StoppingDistance = _stoppingDistance;
            _aiMoveAgent.Speed = _speed;
        }

        public void OnExit()
        { }

        public void Tick()
        {
            Vector3 directionToPlayer = _aiMoveAgent.transform.position - _player.transform.position;
            Vector3 newPosition = _aiMoveAgent.transform.position + directionToPlayer;

            _aiMoveAgent.SetDestination(newPosition);
        }

        public bool IsPlayerClose()
        {
            return Vector3.Distance(_player.transform.position, _aiMoveAgent.transform.position) < _escapingDistance;
        }

        public bool IsPlayerFar()
        {
            return Vector3.Distance(_player.transform.position, _aiMoveAgent.transform.position) > _escapingDistance * EscapeMultiplier;
        }
    }
}