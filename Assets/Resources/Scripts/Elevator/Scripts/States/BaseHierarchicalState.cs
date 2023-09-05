using System;
using UnityEngine;

namespace Features.Lift
{
    public abstract class BaseHierarchicalState
    {
        private bool _isRootState = false;
        private readonly Lift _ctx;
        private readonly StatesFactory _factory;
        private BaseHierarchicalState _currentSubState;
        private BaseHierarchicalState _currentSuperState;

        protected Transform _upPoint;
        protected Transform _downPoint;
        protected Transform _platform;
        protected Animator _animator;

        protected bool IsRootState { set => _isRootState = value; }
        protected Lift Ctx => _ctx;
        protected StatesFactory Factory => _factory;

        protected BaseHierarchicalState(Lift currentContext,
                               Transform upPoint,
                               Transform downPoint,
                               Transform platform,
                               Animator animator,
                               StatesFactory stateFactory)
        {
            _ctx = currentContext;
            _factory = stateFactory;

            _upPoint = upPoint;
            _downPoint = downPoint;
            _platform = platform;
            _animator = animator;
        }

        protected virtual void OnEnter() { }
        protected virtual void OnUpdate() { }
        protected virtual void OnFixedUpdate() { }
        protected virtual void OnExit() { }

        public void EnterState()
        {
            OnEnter();
            _currentSubState?.EnterState();
        }

        public void UpdateState()
        {
            OnUpdate();
            _currentSubState?.UpdateState();
        }

        public void FixedUpdateState()
        {
            OnFixedUpdate();
            _currentSubState?.FixedUpdateState();
        }

        public void ExitState()
        {
            _currentSubState?.ExitState();
            OnExit();
        }

        protected void SwitchState(BaseHierarchicalState newState)
        {
            ExitState();
            newState.EnterState();

            if (_isRootState)
                _ctx.CurrentState = newState;
            else
                _currentSuperState?.SetSubState(newState);
        }

        protected void SetSuperState(BaseHierarchicalState newSuperState)
        {
            _currentSuperState = newSuperState;
        }

        protected void SetSubState(BaseHierarchicalState newSubState)
        {
            _currentSubState = newSubState;
            newSubState.SetSuperState(this);
        }

        protected virtual void OnGetAnimationEvent(string param) { }
        protected virtual void OnCall() { }

        public void GetAnimationEvent(string param)
        {
            OnGetAnimationEvent(param);
            _currentSubState?.GetAnimationEvent(param);
        }

        public void Call()
        {
            OnCall();
            _currentSubState?.Call();
        }
    }


}
