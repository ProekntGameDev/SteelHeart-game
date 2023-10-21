using UnityEngine.InputSystem;
using NaughtyAttributes;
using UnityEngine;
using Zenject;
using System.Collections;

public class PlayerFinisher : MonoBehaviour
{
    [SerializeField, Required] private OverlapSphere _overlapSphere;
    [SerializeField] private Animator _playerAnimator;

    [Inject] private Player _player;

    private Coroutine _finishCoroutine;

    private void OnInteract(InputAction.CallbackContext context)
    {
        foreach (Collider collider in _overlapSphere.GetColliders())
        {
            if (collider.TryGetComponent(out IFinishable finishable))
            {
                if (_finishCoroutine != null)
                    return;

                _finishCoroutine = StartCoroutine(FinishCoroutine(finishable));
                return;
            }
        }
    }

    private IEnumerator FinishCoroutine(IFinishable finishable)
    {
        if (finishable.TryFinish() == false)
        {
            OnEndFisnish();
            yield break;
        }

        OnStartFinish(finishable);

        yield return new WaitForSeconds(finishable.FinishAnimation.Duration);

        OnEndFisnish();
    }

    private void OnStartFinish(IFinishable finishable)
    {
        _player.Interactor.enabled = false;
        _player.Movement.enabled = false;
        _player.Combat.enabled = false;

        _playerAnimator.Play(finishable.FinishAnimation.PlayerState);
    }

    private void OnEndFisnish()
    {
        _finishCoroutine = null;

        _player.Interactor.enabled = true;
        _player.Movement.enabled = true;
        _player.Combat.enabled = true;

        _player.Stamina.Restore(_player.Stamina.Maximum - _player.Stamina.Current);
    }

    private void OnEnable()
    {
        _player.Input.Player.Interact.performed += OnInteract;
    }

    private void OnDisable()
    {
        _player.Input.Player.Interact.performed -= OnInteract;
    }
}
