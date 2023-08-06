using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LevelFailedScreen : MonoBehaviour
{
    [SerializeField, Required] private GameObject _levelFailedScreen;
    [SerializeField, Required] private Button _restartButton;
    [SerializeField, Required] private Button _mainMenuButton;


    [Inject] private SceneManager _sceneManager;
    [Inject] private Player _player;

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
