using NaughtyAttributes;
using UnityEngine;

namespace AI
{
    public class DroneState_Freeze : MonoBehaviour, IState
    {
        [SerializeField, Required] private AIMoveAgent _aiMoveAgent;
        [SerializeField, Required] private DroneState_Attack _attackState;

        [SerializeField] private float _minHeight;
        [SerializeField] private float _duration;

        private float _maxHeight;
        private float _endTime;

        public void OnEnter()
        {
            _maxHeight = _aiMoveAgent.BaseOffset;

            _endTime = Time.time + _duration;

            _aiMoveAgent.BaseOffset = _minHeight;
            _aiMoveAgent.IsStopped = true;
        }

        public void OnExit()
        {
            _endTime = 0;

            _attackState.Reload();

            _aiMoveAgent.BaseOffset = _maxHeight;
            _aiMoveAgent.IsStopped = false;
        }

        public void Tick()
        { }

        public bool IsDone() => _endTime <= Time.time;
    }
}