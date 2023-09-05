using UnityEngine;

namespace Features.Lift
{
    public class OpenedDoorState : BaseHierarchicalState
    {
        private float _elapsed;

        public OpenedDoorState(Lift currentContext,
                               Transform upPoint,
                               Transform downPoint,
                               Transform platform,
                               Animator animator,
                               StatesFactory stateFactory)
            : base(currentContext, upPoint, downPoint, platform, animator, stateFactory)
        {
            IsRootState = true;
        }

        protected override void OnEnter()
        {
            TryResetCalls();
            _elapsed = 0;
        }

        protected override void OnUpdate()
        {
            _elapsed += Time.deltaTime;

            if (_elapsed < Ctx.MinOpenedTime)
                return;

            CheckCalls();
        }

        protected override void OnCall()
        {
            TryResetCalls();
        }

        private void CheckCalls()
        {
            if (Ctx.DownLevel && Ctx.HasCallToUp)
                SwitchState(Factory.GetSate<ClosingDoorState>());

            if (Ctx.UpLevel && Ctx.HasCallToDown)
                SwitchState(Factory.GetSate<ClosingDoorState>());
        }

        private void TryResetCalls()
        {
            if (Ctx.DownLevel)
                Ctx.HasCallToDown = false;

            if (Ctx.UpLevel)
                Ctx.HasCallToUp = false;
        }
    }


}
