using UnityEngine;
using Zenject;

public class ProjectMonoInstaller : MonoInstaller
{
    [SerializeField] private SceneLoadScreen _sceneLoadScreen;
    [SerializeField] private SceneManager _sceneManager;
    [SerializeField] private SaveManager _saveManager;

    public override void InstallBindings()
    {
        Container.Bind<SceneLoadScreen>().FromInstance(_sceneLoadScreen).AsSingle();
        Container.Bind<SceneManager>().FromInstance(_sceneManager).AsSingle();
        Container.Bind<SaveManager>().FromInstance(_saveManager).AsSingle();
    }
}