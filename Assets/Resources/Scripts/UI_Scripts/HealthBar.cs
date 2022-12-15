using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;

[RequireComponent(typeof(Image))]
public class HealthBar : MonoBehaviour
{
    [SerializeField] private Player player;
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
        player.Health.OnHealthChanged.AddListener(OnHealthChanged);
    }

    private void OnHealthChanged(float newValue)
    {
        var value = newValue / player.Health.MaxValue;
        // _image.color = new Color(255 - 255 * value, 255 * value, 0, 255); // color change to redder with decreased health. Has bugs
        _image.fillAmount = value;
    }
}