using UnityEngine;

namespace Features.Lift
{
    public class ClosedDoorState : BaseHierarchicalState
    {
        public ClosedDoorState(Lift lift,
                              Transform upPoint,
                              Transform downPoint,
                              Transform platform,
                              Animator animator,
                              StatesFactory factory)
            : base(lift, upPoint, downPoint, platform, animator, factory)
        {
            IsRootState = true;
        }

        protected override void OnUpdate()
        {
            CheckCalls();
        }

        private void CheckCalls()
        {
            if (Ctx.DownLevel)
            {
                if (Ctx.HasCallToDown)
                    SwitchState(Factory.GetSate<OpeningDoorState>());
                else if (Ctx.HasCallToUp)
                    SwitchState(Factory.GetSate<MovingState>());
            }

            if (Ctx.UpLevel)
            {
                if (Ctx.HasCallToUp)
                    SwitchState(Factory.GetSate<OpeningDoorState>());
                else if (Ctx.HasCallToDown)
                    SwitchState(Factory.GetSate<MovingState>());
            }
        }
    }


}
