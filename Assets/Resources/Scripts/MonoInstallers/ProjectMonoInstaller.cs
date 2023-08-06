using UnityEngine;
using Zenject;

public class ProjectMonoInstaller : MonoInstaller
{
    [SerializeField] private SceneManager _sceneManager;
    [SerializeField] private SaveManager _saveManager;

    public override void InstallBindings()
    {
        Container.Bind<SceneManager>().FromInstance(_sceneManager).AsSingle();
        Container.Bind<SaveManager>().FromInstance(_saveManager).AsSingle();
    }
}