using UnityEngine;

namespace Features.Lift
{
    public class MovingState : BaseHierarchicalState
    {
        public bool IsMoveUp { get; private set; }

        private float _elapsed;
        private float _timeToEndPoint;

        public MovingState(Lift lift,
                           Transform upPoint,
                           Transform downPoint,
                           Transform platform,
                           Animator animator,
                           StatesFactory factory) : base(lift, upPoint, downPoint, platform, animator, factory)
        {
            IsRootState = true;
        }

        protected override void OnEnter()
        {
            SetTiming();
            CheckDirection();
        }

        protected override void OnUpdate()
        {
            //Move();
        }

        protected override void OnFixedUpdate()
        {
            Move();
        }

        protected override void OnExit()
        {
            if (IsMoveUp)
                _platform.position = _upPoint.position;
            else
                _platform.position = _downPoint.position;
        }

        private void SetTiming()
        {
            _elapsed = 0;
            float distance = Vector3.Distance(_downPoint.position, _upPoint.position);
            _timeToEndPoint = distance / Ctx.Speed;
        }

        private void CheckDirection()
        {
            if (Ctx.DownLevel)
                IsMoveUp = true;

            if (Ctx.UpLevel)
                IsMoveUp = false;
        }

        private void Move()
        {
            _elapsed += Time.deltaTime;

            float elapsedPercentage = _elapsed / _timeToEndPoint;
            elapsedPercentage = Mathf.SmoothStep(0, 1, elapsedPercentage);

            if (IsMoveUp)
                _platform.position = Vector3.Lerp(_downPoint.position, _upPoint.position, elapsedPercentage);
            else
                _platform.position = Vector3.Lerp(_upPoint.position, _downPoint.position, elapsedPercentage);

            if (elapsedPercentage >= 1)
                SwitchState(Factory.GetSate<ClosedDoorState>());
        }
    }


}
