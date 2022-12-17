using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthBar : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI healthPercent;

    private HealthOld _playerHealth;

    private void OnEnable()
    {
        _playerHealth.OnChange += Fill;
    }

    private void OnDisable()
    {
        _playerHealth.OnChange -= Fill;
    }

    private void Awake()
    {
        var playerController = FindObjectOfType<PlayerCN>();
        _playerHealth = playerController.GetComponent<HealthOld>();        
    }

    private void Fill()
    {
        float percentage = _playerHealth.Percentage;
        image.fillAmount = percentage;
        healthPercent.text = $"{(int)(percentage*100)}%";
    }
}
