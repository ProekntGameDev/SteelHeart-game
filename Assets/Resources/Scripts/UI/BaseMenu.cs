using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public abstract class BaseMenu : MonoBehaviour
{
    protected abstract InputAction _menuButton { get; }

    [SerializeField, Required] protected GameObject _panel;

    [Inject] protected Player _player;

    private void OnEnable()
    {
        _menuButton.performed += Toogle;
        _player.Input.PlayerUI.Pause.performed += EscapePerformed;
    }

    private void OnDisable()
    {
        _menuButton.performed -= Toogle;
        _player.Input.PlayerUI.Pause.performed -= EscapePerformed;
    }

    protected virtual void Enable()
    {
        _panel.SetActive(true);
        _player.Input.Player.Disable();
    }

    protected virtual void Disable()
    {
        _panel.SetActive(false);
        _player.Input.Player.Enable();
    }

    private void Toogle(InputAction.CallbackContext context)
    {
        if (_panel.activeInHierarchy)
            Disable();
        else
            Enable();
    }

    protected abstract void EscapePerformed(InputAction.CallbackContext context);
}
