using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerStaminaBar : MonoBehaviour
{
    [SerializeField] private Image _image;

    [Inject] private Player _player;

    private void Awake()
    {
        _player.Stamina.OnChange.AddListener(OnStaminaChanged);
    }

    private void OnStaminaChanged()
    {
        var value = _player.Stamina.Current / _player.Stamina.Maximum;
        _image.fillAmount = value;
    }
}
