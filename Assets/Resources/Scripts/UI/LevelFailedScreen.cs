using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class LevelFailedScreen : MonoBehaviour
{
    [SerializeField, Required] private SceneManager _sceneManager;
    [SerializeField, Required] private Player _player;
    [SerializeField, Required] private GameObject _levelFailedScreen;
    [SerializeField, Required] private Button _restartButton;
    [SerializeField, Required] private Button _mainMenuButton;

    private void Start()
    {
        _player.Health.OnDeath.AddListener(OnPlayerDie);

        _restartButton.onClick.AddListener(_sceneManager.ReloadCurrent);
        _mainMenuButton.onClick.AddListener(_sceneManager.LoadMenu);
    }

    private void OnPlayerDie()
    {
        _levelFailedScreen.SetActive(true);
    }
}
