using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PauseScreen : MonoBehaviour
{
    [SerializeField, Required] private GameObject _pausePanel;
    [SerializeField, Required] private Button _continueButton;
    [SerializeField, Required] private Button _saveButton;
    [SerializeField, Required] private Button _loadButton;
    [SerializeField, Required] private Button _settingsButton;
    [SerializeField, Required] private Button _mainMenuButton;

    [Inject] private Player _player;
    [Inject] private SceneManager _sceneManager;

    private void Start()
    {
        _continueButton.onClick.AddListener(Disable);
        _mainMenuButton.onClick.AddListener(LoadMenu);
    }

    private void OnEnable()
    {
        _player.Input.Player.Pause.performed += (context) => Enable();
    }

    private void OnDisable()
    {
        _player.Input.Player.Pause.performed -= (context) => Enable();
    }

    private void Enable()
    {
        _pausePanel.SetActive(true);

        _player.Input.Player.Disable();
        Time.timeScale = 0;
    }

    private void Disable()
    {
        _pausePanel.SetActive(false);

        _player.Input.Player.Enable();
        Time.timeScale = 1;
    }

    private void LoadMenu()
    {
        _player.Input.Player.Enable();
        Time.timeScale = 1;

        _sceneManager.LoadMenu();
    }
}
