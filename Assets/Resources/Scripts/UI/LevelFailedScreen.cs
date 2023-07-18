using NaughtyAttributes;
using UnityEngine;

public class LevelFailedScreen : MonoBehaviour
{
    [SerializeField, Required] private Player _player;
    [SerializeField, Required] private GameObject _levelFailedScreen;

    private void Start()
    {
        _player.Health.OnDeath.AddListener(OnPlayerDie);
    }

    private void OnPlayerDie()
    {
        _levelFailedScreen.SetActive(true);
    }
}
