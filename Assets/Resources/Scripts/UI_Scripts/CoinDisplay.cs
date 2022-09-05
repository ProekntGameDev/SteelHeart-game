using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoinDisplay : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private PlayerCoinHolder _coinHolder;


    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _coinHolder = FindObjectOfType<PlayerCoinHolder>();
    }

    private void OnEnable()
    {
        _coinHolder.OnChange += UpdateCoins;
    }

    private void OnDisable()
    {
        _coinHolder.OnChange -= UpdateCoins;
    }

    private void UpdateCoins()
    {
        _text.text = _coinHolder.Coins.ToString();
    }
}
