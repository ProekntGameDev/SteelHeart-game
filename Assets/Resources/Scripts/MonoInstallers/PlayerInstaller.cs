using NaughtyAttributes;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(PlayerDataLoader))]
public class PlayerInstaller : MonoInstaller
{
    [SerializeField, Required] private Player _player;
    [SerializeField, Required] private Camera _mainCamera;

    private PlayerDataLoader _playerDataLoader;

    public override void InstallBindings()
    {
        Container.Bind<Player>().FromInstance(_player).AsSingle();
        Container.Bind<Camera>().FromInstance(_mainCamera).AsSingle();
    }

    private void Awake()
    {
        _playerDataLoader = GetComponent<PlayerDataLoader>();
    }

    public override void Start()
    {
        base.Start();
        _playerDataLoader.Load(_player);
    }
}