using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class LevelPassedScreen : MonoBehaviour
{
    [SerializeField, Required] private SceneManager _sceneManager;
    [SerializeField, Required] private LevelEndTrigger _levelEndTrigger;
    [SerializeField, Required] private GameObject _levelPassedScreen;
    [SerializeField, Required] private Button _continueButton;
    [SerializeField, Required] private Button _restartButton;
    [SerializeField, Required] private Button _mainMenuButton;

    private void Start()
    {
        _levelEndTrigger.OnLevelEnd.AddListener(OnTriggerReached);

        _continueButton.onClick.AddListener(_sceneManager.LoadNext);
        _restartButton.onClick.AddListener(_sceneManager.ReloadCurrent);
        _mainMenuButton.onClick.AddListener(_sceneManager.LoadMenu);
    }

    private void OnTriggerReached()
    {
        _levelPassedScreen.SetActive(true);
    }
}
