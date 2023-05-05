using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthBar : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI healthPercent;

    private Health _playerHealth;

    private void Awake()
    {
        var playerController = FindObjectOfType<PlayerCN>();
        _playerHealth = playerController.GetComponent<Health>();

        _playerHealth.OnChange.AddListener(UpdateBar);
    }

    private void UpdateBar(float health)
    {
        float percentage = health / _playerHealth.Max;
        image.fillAmount = percentage;
        healthPercent.text = $"{(int)(percentage*100)}%";
    }
}
