using UnityEngine;
using NaughtyAttributes;
using Zenject;

public class PlayerDataLoader : MonoBehaviour
{
    [SerializeField] private bool _loadAdditionalData;
    [SerializeField, ShowIf(nameof(_loadAdditionalData))] private Camera _mainCamera;
    [SerializeField, ShowIf(nameof(_loadAdditionalData))] private Transform _relativeObject;

    [Inject] private Player _player;
    [Inject] private SaveManager _saveManager;

    private void Awake()
    {
        Load();
    }

    private void Load()
    {
        PlayerSaveData playerSaveData = _saveManager.Load();

        playerSaveData.Load(_player);

        if (_loadAdditionalData == false)
            return;

        playerSaveData.AdditionalData.Load(_player, _relativeObject.position, _mainCamera);
    }
}
