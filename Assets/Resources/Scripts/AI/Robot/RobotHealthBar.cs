using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Zenject;

public class RobotHealthBar : Interactable
{
    [SerializeField, Required] private Health _health;
    [SerializeField, Required] private Canvas _healthBarCanvas;
    [SerializeField, Required] private GameObject _finishHint;
    [SerializeField, Required] private Image _healthBarImage;
    [SerializeField, Required] private TextMeshProUGUI _healthBarText;

    [Inject] private Camera _targetCamera;

    public override void OnSelect()
    {
        _healthBarCanvas.gameObject.SetActive(true);
    }

    public override void OnUnselect()
    {
        _healthBarCanvas.gameObject.SetActive(false);
    }

    private void Start()
    {
        _health.OnChange.AddListener(UpdateHealthBar);

        UpdateHealthBar(_health.Current);

        _healthBarCanvas.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_healthBarCanvas.gameObject.activeInHierarchy == false)
            return;

        Vector3 forward = _healthBarCanvas.transform.position - _targetCamera.transform.rotation * Vector3.back;

        _healthBarCanvas.transform.LookAt(forward, _targetCamera.transform.rotation * Vector3.up);
    }

    private void UpdateHealthBar(float newValue)
    {
        if (newValue == 0)
            _healthBarImage.gameObject.SetActive(false);

        float healthPercent = newValue / _health.Max;

        _healthBarImage.fillAmount = healthPercent;
        _healthBarText.text = $"{(int)(healthPercent * 100)}%";
    }
}
