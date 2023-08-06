using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LevelPassedScreen : MonoBehaviour
{
    [SerializeField, Required] private Trigger _levelEndTrigger;
    [SerializeField, Required] private GameObject _levelPassedScreen;
    [SerializeField, Required] private Button _continueButton;
    [SerializeField, Required] private Button _restartButton;
    [SerializeField, Required] private Button _mainMenuButton;

    [Inject] private SceneManager _sceneManager;

    private void Start()
    {
        _levelEndTrigger.OnInteract.AddListener(OnTriggerReached);

        _continueButton.onClick.AddListener(_sceneManager.LoadNext);
        _restartButton.onClick.AddListener(_sceneManager.ReloadCurrent);
        _mainMenuButton.onClick.AddListener(_sceneManager.LoadMenu);
    }

    private void OnTriggerReached()
    {
        _levelPassedScreen.SetActive(true);
    }
}
