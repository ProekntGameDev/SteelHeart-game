using UnityEngine;
using NaughtyAttributes;
using Zenject;
using Features.Lift;
using System.Collections;

public class SceneLift : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _minOpenedTime;
    [SerializeField] private float _timeToEndAcceleration;

    [SerializeField, Required] private Animator _animator;
    [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Trigger)] private string _openDoors;
    [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Trigger)] private string _closeDoors;

    [SerializeField, Required] private Camera _mainCamera;
    [SerializeField, Required] private Transform _platform;

    [SerializeField] private Interactable _openLiftButton;
    [SerializeField] private Interactable _moveButton;

    [Inject] private Player _player;
    [Inject] private SaveManager _saveManager;
    [Inject] private SceneManager _sceneManager;

    private bool _isLoadingScene;
    private bool _isMoving;
    private float _elapsed;
    private float _elapsedPercentage;

    private void Awake()
    {
        _moveButton.OnInteract.AddListener(() => StartCoroutine(StartMove()));

        _openLiftButton.OnInteract.AddListener(() => _animator.SetTrigger(_openDoors));
    }

    private IEnumerator StartMove()
    {
        if (_isMoving)
            yield break;

        _animator.SetTrigger(_closeDoors);

        yield return new WaitForSeconds(_minOpenedTime);

        _elapsed = 0;
        _isMoving = true;
    }

    private void FixedUpdate()
    {
        if (_isMoving == false)
            return;

        _elapsed += Time.deltaTime;

        _elapsedPercentage = _elapsed / _timeToEndAcceleration;
        _elapsedPercentage = Mathf.SmoothStep(0, 1, _elapsedPercentage);

        _platform.Translate(Vector3.up * _speed * _elapsedPercentage * Time.fixedDeltaTime);

        if (_isLoadingScene == false && _elapsedPercentage == 1)
        {
            StartCoroutine(LoadScene());
            _isLoadingScene = true;
        }
    }

    private void OnAnimationEvent(string param)
    { }

    private IEnumerator LoadScene()
    {
        AsyncOperation operation = _sceneManager.LoadNext(false);

        operation.allowSceneActivation = false;

        yield return new WaitWhile(() => operation.progress < 0.9f);

        operation.allowSceneActivation = true;

        _saveManager.Save(GetSaveData());
    }

    private PlayerSaveData GetSaveData()
    {
        PlayerMovementData movementData = new PlayerMovementData(_player, _player.transform.position - _platform.position, _mainCamera);
        return new PlayerSaveData(_player, _sceneManager.NextScene(), movementData);
    }
}
