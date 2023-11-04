using NaughtyAttributes;
using UnityEngine;

namespace AI
{
    public class RobotState_Patrol : MonoBehaviour, IState
    {
        [SerializeField, Required] private AIMoveAgent _aiMoveAgent;
        [SerializeField] private Transform[] _points;
        [SerializeField] private float _speed;

        private int _targetPointIndex;

        public void OnEnter()
        {
            _aiMoveAgent.Speed = _speed;

            _targetPointIndex++;

            if (_targetPointIndex >= _points.Length)
                _targetPointIndex = 0;

            _aiMoveAgent.UpdateRotation = true;
            _aiMoveAgent.SetDestination(_points[_targetPointIndex].position);
        }

        public void OnExit()
        { }

        public void Tick()
        { }

        public bool IsDone() => _aiMoveAgent.IsDone();
    }
}
