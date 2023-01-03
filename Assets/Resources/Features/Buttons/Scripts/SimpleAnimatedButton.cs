using UnityEngine;

namespace Features.Buttons
{
    [RequireComponent(typeof(Animator))]
    public class SimpleAnimatedButton : AbstractActivatedSource
    {
        [SerializeField] private string _animationTriggerName = "OnPress";

        private Animator _animator;
        private int _animationTriggerHash;

        private bool _pressed = false;

        private void Awake()
        {
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
