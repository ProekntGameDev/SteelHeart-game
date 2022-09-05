using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthBar : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI healthPercent;

    private Health _playerHealth;

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
        var playerController = FindObjectOfType<PlayerMovementController>();
        _playerHealth = playerController.GetComponent<Health>();        
    }

    private void Fill()
    {
        float percentage = _playerHealth.Percentage;
        image.fillAmount = percentage;
        healthPercent.text = $"{(int)(percentage*100)}%";
    }
}
