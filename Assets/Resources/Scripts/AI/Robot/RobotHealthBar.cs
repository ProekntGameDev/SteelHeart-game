using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;

public class RobotHealthBar : MonoBehaviour
{
    [SerializeField, Required] private Camera _targetCamera;
    [SerializeField, Required] private Health _health;
    [SerializeField, Required] private Image _healthBarImage;
    [SerializeField, Required] private TextMeshProUGUI _healthBarText;

    private void Start()
    {
        _health.OnChange.AddListener(UpdateHealthBar);

        UpdateHealthBar(_health.Current);
    }

    private void Update()
    {
        Vector3 forward = transform.position - _targetCamera.transform.rotation * Vector3.back;

        gameObject.transform.LookAt(forward, _targetCamera.transform.rotation * Vector3.up);
    }

    private void UpdateHealthBar(float newValue)
    {
        float healthPercent = newValue / _health.Max;

        _healthBarImage.fillAmount = healthPercent;
        _healthBarText.text = $"{(int)(healthPercent * 100)}%";
    }
}
