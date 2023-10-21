using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace AI
{
    public class RobotState_Chase : MonoBehaviour, IState
    {
        [SerializeField, Required] private NavMeshAgent _navMeshAgent;
        [SerializeField] private RobotVision _robotVision;
        [SerializeField] private float _speed;
        [SerializeField] private float _minDistance;
        [SerializeField] private float _maxDistance;

        [Inject] private Player _player;

        private float _baseStoppingDistance;

        public void OnEnter()
        {
            _navMeshAgent.speed = _speed;
            _navMeshAgent.updateRotation = true;

            _baseStoppingDistance = _navMeshAgent.stoppingDistance;
            _navMeshAgent.stoppingDistance = _minDistance;
        }

        public void OnExit()
        {
            _navMeshAgent.stoppingDistance = _baseStoppingDistance;
            _player = null;
        }

        public void Tick()
        {
            if (_robotVision.IsVisible(out _player))
                _navMeshAgent.destination = _player.transform.position;
        }

        public bool IsLostPlayer()
        {
            return _player == null || Vector3.Distance(_player.transform.position, _navMeshAgent.transform.position) > _maxDistance;
        }

        public bool IsDone()
        {
            return _navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete && _player != null;
        }
    }
}
