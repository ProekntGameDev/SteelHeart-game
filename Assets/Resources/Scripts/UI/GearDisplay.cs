using UnityEngine;
using TMPro;
using NaughtyAttributes;

[RequireComponent(typeof(TextMeshProUGUI))]
public class GearDisplay : MonoBehaviour
{
    [SerializeField, Required] private Player _player;

    private TextMeshProUGUI _text;

    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();

        _player.GearsHolder.OnChange.AddListener(UpdateGears);
    }

    private void UpdateGears(int value)
    {
        _text.text = value.ToString();
    }
}
