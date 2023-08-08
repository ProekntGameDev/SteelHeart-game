using Zenject;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [Inject] private SceneManager _sceneManager;

    public void LoadNext() => _sceneManager.LoadNext();
    public void Quit() => _sceneManager.Quit();
}
