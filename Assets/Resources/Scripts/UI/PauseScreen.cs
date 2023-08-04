using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{
    [SerializeField, Required] private Player _player;
    [SerializeField, Required] private SceneManager _sceneManager;
    [SerializeField, Required] private GameObject _pausePanel;
    [SerializeField, Required] private Button _continueButton;
    [SerializeField, Required] private Button _saveButton;
    [SerializeField, Required] private Button _loadButton;
    [SerializeField, Required] private Button _settingsButton;
    [SerializeField, Required] private Button _mainMenuButton;

    private void Start()
    {
        _continueButton.onClick.AddListener(Disable);
        _mainMenuButton.onClick.AddListener(_sceneManager.LoadMenu);
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
}
