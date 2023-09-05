using NaughtyAttributes;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(PlayerDataLoader))]
public class PlayerInstaller : MonoInstaller
{
    [SerializeField, Required] private Player _player;

    public override void InstallBindings()
    {
        Container.Bind<Player>().FromInstance(_player).AsSingle();
    }

    private void Awake()
    {
        PlayerDataLoader playerDataLoader = GetComponent<PlayerDataLoader>();

        playerDataLoader.Load(_player);
    }
}