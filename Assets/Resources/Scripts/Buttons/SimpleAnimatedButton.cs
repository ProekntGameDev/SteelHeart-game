using UnityEngine;
using NaughtyAttributes;

namespace Features.Buttons
{
    [RequireComponent(typeof(Animator))]
    public class SimpleAnimatedButton : InteractableTrigger
    {
        [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Trigger)] private string _animationTriggerName = "OnPress";
        [SerializeField, Required] private Animator _animator;

        private bool _pressed = false;

        private void Awake()
        {
            if(_animator == null)
                _animator = GetComponent<Animator>();

            OnInteract.AddListener(TryInteract);
        }

        private void TryInteract()
        {
            if (_pressed)
                return;

            _pressed = true;
            _animator.SetTrigger(_animationTriggerName);
        }

        private void OnAnimationComplete()
        {
            _pressed = false;
        }
    }
}
