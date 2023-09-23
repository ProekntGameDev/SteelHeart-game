using NaughtyAttributes;
using UnityEngine;
using Zenject;

public class PlayerDataLoader : MonoBehaviour
{
    [SerializeField] private bool _loadAdditionalData;
    [SerializeField, ShowIf(nameof(_loadAdditionalData))] private Camera _mainCamera;
    [SerializeField, ShowIf(nameof(_loadAdditionalData))] private Transform _relativeObject;

    [Inject] private SaveManager _saveManager;

    public void Load(Player player)
    {
        if (_saveManager.GetSaves().Length == 0)
            return;

        PlayerSaveData playerSaveData = _saveManager.Load();

#if UNITY_EDITOR
        if (playerSaveData.Scene != UnityEngine.SceneManagement.SceneManager.GetActiveScene().name)
            return;
#endif

        playerSaveData.Load(player);

        if (_loadAdditionalData == false)
            return;

        playerSaveData.AdditionalData.Load(player, _relativeObject.position, _mainCamera);
    }
}
