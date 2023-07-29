using UnityEngine;
using NaughtyAttributes;

namespace Features.Buttons
{
    [RequireComponent(typeof(Animator))]
    public class SimpleAnimatedButton : AbstractActivatedSource
    {
        [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Trigger)] private string _animationTriggerName = "OnPress";
        [SerializeField, Required] private Animator _animator;

        private int _animationTriggerHash;

        private bool _pressed = false;

        private void Awake()
        {
            if(_animator == null)
                _animator = GetComponent<Animator>();
            _animationTriggerHash = Animator.StringToHash(_animationTriggerName);
        }

        protected override bool TryInteract()
        {
            if (_pressed)
                return false;

            _pressed = true;
            PlayAnimation();
            return true;
        }

        private void OnAnimationComplete()
        {
            _pressed = false;
        }

        private void PlayAnimation()
        {
            _animator.SetTrigger(_animationTriggerHash);
        }
    }
}
