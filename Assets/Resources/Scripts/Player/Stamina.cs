using System;
using UnityEngine;

public class Stamina : MonoBehaviour
{
    public event Action OnChange;

    public float Maximum => _maximum;
    public float Current => _current;

    [SerializeField] private float _maximum = 10;
    [SerializeField] private float _restorationRate = 1f;
    
    private float _current;

    private void Start()
    {
        if(_current == 0)
            _current = _maximum;
    }

    public void DecayFixedTime(float amount)
    {
        _current -= amount * Time.fixedDeltaTime;
        if (_current < 0) _current = 0;
        OnChange?.Invoke();
    }

    public void RestoreFixedTime()
    {
        if (_current == _maximum) return;

        _current += _restorationRate * Time.fixedDeltaTime;
        if (_current > _maximum) _current = _maximum;
        OnChange?.Invoke();
    }

    public void Load(PlayerSaveData data)
    {
        _current = data.Stamina;
        OnChange?.Invoke();
    }
}
