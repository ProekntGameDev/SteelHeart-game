using UnityEngine;
using TMPro;
using NaughtyAttributes;

[RequireComponent(typeof(TextMeshProUGUI))]
public class CoinDisplay : MonoBehaviour
{
    [SerializeField, Required] private Player _player;

    private TextMeshProUGUI _text;

    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();

        _player.CoinHolder.OnChange.AddListener(UpdateCoins);
    }

    private void UpdateCoins(int value)
    {
        _text.text = value.ToString();
    }
}
