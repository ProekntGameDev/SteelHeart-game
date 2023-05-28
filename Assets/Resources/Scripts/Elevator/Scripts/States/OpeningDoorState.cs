using UnityEngine;

namespace Features.Lift
{
    public class OpeningDoorState : BaseHierarchicalState
    {
        public OpeningDoorState(Lift currentContext,
                                Transform upPoint,
                                Transform downPoint,
                                Transform platform,
                                Animator animator,
                                StatesFactory stateFactory) : base(currentContext, upPoint, downPoint, platform, animator, stateFactory)
        {
            IsRootState = true;
        }

        protected override void OnEnter()
        {
            _animator.SetTrigger("Open");
        }

        protected override void OnGetAnimationEvent(string param)
        {
            if (param == "opened")
                SwitchState(Factory.GetSate<OpenedDoorState>());
        }
    }


}
