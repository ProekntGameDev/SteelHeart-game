using Zenject;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [Inject] private SceneManager _sceneManager;
    [Inject] private SaveManager _saveManager;

    public void NewGame()
    {
        if (_saveManager.GetSaves().Length > 0)
            _saveManager.Delete();

        _sceneManager.LoadNext();
    }

    public void LoadFromSave()
    {
        if (_saveManager.GetSaves().Length == 0)
            return;

        _sceneManager.LoadFromSave();
    }

    public void Quit() => _sceneManager.Quit();
}
