using NaughtyAttributes;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(PlayerDataLoader))]
public class PlayerInstaller : MonoInstaller
{
    [SerializeField, Required] private Player _player;

    private PlayerDataLoader _playerDataLoader;

    public override void InstallBindings()
    {
        Container.Bind<Player>().FromInstance(_player).AsSingle();
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