using UnityEngine;
using TMPro;
using NaughtyAttributes;

[RequireComponent(typeof(TextMeshProUGUI))]
public class CoinDisplay : MonoBehaviour
{
    [SerializeField, Required] private PlayerCoinHolder _coinHolder;

    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();

        _coinHolder.OnChange.AddListener(UpdateCoins);
    }

    private void UpdateCoins(int value)
    {
        _text.text = value.ToString();
    }
}
