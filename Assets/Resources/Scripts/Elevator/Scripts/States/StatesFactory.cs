using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.Lift
{
    public class StatesFactory
    {
        private readonly Dictionary<Type, BaseHierarchicalState> _states = new Dictionary<Type, BaseHierarchicalState>();

        protected Lift _lift;
        protected Transform _upPoint;
        protected Transform _downPoint;
        protected Transform _platform;
        protected Animator _animator;

        public StatesFactory(Lift lift,
                               Transform upPoint,
                               Transform downPoint,
                               Transform platform,
                               Animator animator)
        {
            _lift = lift;
            _upPoint = upPoint;
            _downPoint = downPoint;
            _platform = platform;
            _animator = animator;

            CreateStates();
        }

        private void CreateStates()
        {
            _states.Add(typeof(ClosedDoorState),
                        new ClosedDoorState(_lift, _upPoint, _downPoint, _platform, _animator, this));

            _states.Add(typeof(OpeningDoorState),
                        new OpeningDoorState(_lift, _upPoint, _downPoint, _platform, _animator, this));

            _states.Add(typeof(OpenedDoorState),
                        new OpenedDoorState(_lift, _upPoint, _downPoint, _platform, _animator, this));

            _states.Add(typeof(ClosingDoorState),
                        new ClosingDoorState(_lift, _upPoint, _downPoint, _platform, _animator, this));

            _states.Add(typeof(MovingState),
                        new MovingState(_lift, _upPoint, _downPoint, _platform, _animator, this));
        }

        public BaseHierarchicalState GetSate<T>() where T : BaseHierarchicalState
        {
            return _states[typeof(T)];
        }
    }


}
