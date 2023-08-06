using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _text;

    [Inject] private Player _player;

    private void Start()
    {
        _player.Health.OnChange.AddListener(OnHealthChanged);
    }

    private void OnHealthChanged(float newValue)
    {
        var value = newValue / _player.Health.Max;
        _image.fillAmount = value;

        var presents = Math.Round(value, 3) * 100;
        _text.text = presents.ToString(CultureInfo.CurrentCulture) + "%";
    }
}