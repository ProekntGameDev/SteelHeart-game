using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace AI
{
    public abstract class RobotBrain : MonoBehaviour
    {
        protected StateMachine _stateMachine { get; private set; }

        [Required, SerializeField] protected NavMeshAgent _navMeshAgent;
        [Required, SerializeField] protected Health _robotHealth;
        [SerializeField] protected RobotVision _robotVision;

        [Inject] protected Player _player;

        protected virtual void Awake()
        {
            _stateMachine = new StateMachine();

            SetupStates();

            SetupTransitions();
        }

        protected virtual void Update()
        {
            _stateMachine.Tick();
        }

        protected abstract void SetupStates();

        protected abstract void SetupTransitions();

        private void OnDrawGizmosSelected()
        {
            _robotVision.OnGizmos();
        }
    }
}
