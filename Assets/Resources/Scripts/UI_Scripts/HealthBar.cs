using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _text;

    private void Awake()
    {
        _player.Health.OnHealthChanged.AddListener(OnHealthChanged);
    }

    private void OnHealthChanged(float newValue)
    {
        var value = newValue / _player.Health.MaxValue;
        _image.fillAmount = value;

        var presents = Math.Round(value, 3) * 100;
        _text.text = presents.ToString(CultureInfo.CurrentCulture) + "%";
    }
}