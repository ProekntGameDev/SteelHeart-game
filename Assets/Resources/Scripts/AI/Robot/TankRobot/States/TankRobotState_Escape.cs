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

        private Vector3 _direction;

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
            directionToPlayer = directionToPlayer.normalized * _escapingDistance;

            for (int i = 0; i < 48; i++)
            {
                _direction = Quaternion.Euler(0, (360 / 48) * i, 0) * directionToPlayer;

                _aiMoveAgent.SetDestination(_aiMoveAgent.transform.position + _direction);
                Debug.DrawLine(_aiMoveAgent.transform.position, _aiMoveAgent.transform.position + _direction);
                if (_aiMoveAgent.CanCalculatePath(_aiMoveAgent.transform.position + _direction))
                    break;
            }
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