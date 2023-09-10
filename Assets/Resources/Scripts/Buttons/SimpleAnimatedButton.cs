using UnityEngine;
using NaughtyAttributes;
using System.Collections;

namespace Features.Buttons
{
    public class SimpleAnimatedButton : Interactable
    {
        [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Trigger)] private string _animationTriggerName = "OnPress";
        [SerializeField, Required] private Animator _animator;
        [SerializeField] private float _delay;

        private bool _pressed = false;

        private void Awake()
        {
            if(_animator == null)
                _animator = GetComponent<Animator>();
        }

        public override void Interact()
        {
            base.Interact();

            if (_pressed)
                return;

            _pressed = true;
            CanInteract = false;
            _animator.SetTrigger(_animationTriggerName);

            StartCoroutine(Delay());
        }

        private IEnumerator Delay()
        {
            yield return new WaitForSeconds(_delay);
            CanInteract = true;
            _pressed = false;
        }
    }
}
