using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

public class PauseScreen : BaseMenu
{
    [SerializeField, Required] private Button _continueButton;
    [SerializeField, Required] private Button _loadButton;
    [SerializeField, Required] private Button _settingsButton;
    [SerializeField, Required] private Button _mainMenuButton;

    [Inject] private SceneManager _sceneManager;

    protected override InputAction _menuButton => _player.Input.PlayerUI.Pause;

    private void Start()
    {
        _continueButton.onClick.AddListener(Disable);
        _mainMenuButton.onClick.AddListener(LoadMenu);
        _loadButton.onClick.AddListener(Load);
    }

    protected override void Enable()
    {
        base.Enable();
        Time.timeScale = 0;
    }

    protected override void Disable()
    {
        base.Disable();
        Time.timeScale = 1;
    }

    private void LoadMenu()
    {
        _player.Input.Player.Enable();
        Time.timeScale = 1;

        _sceneManager.LoadMenu();
    }

    private void Load()
    {
        _player.Input.Player.Enable();
        Time.timeScale = 1;

        _sceneManager.ReloadCurrent();
    }

    protected override void EscapePerformed(InputAction.CallbackContext context)
    {
    }
}
