using System;
using UnityEngine;
using UnityEngine.Events;

public class Stamina : MonoBehaviour
{
    [HideInInspector] public UnityEvent OnChange;

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

    public void Decay(float amount)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException(nameof(amount));

        _current -= amount;

        if (_current < 0) 
            _current = 0;

        OnChange?.Invoke();
    }

    public void Restore(float amount)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException(nameof(amount));

        _current += amount;

        _current = Mathf.Min(_current, _maximum);

        OnChange?.Invoke();
    }

    public void RestoreFixedTime(float multiplier)
    {
        if (multiplier < 0)
            throw new ArgumentOutOfRangeException(nameof(multiplier));

        _current += _restorationRate * multiplier * Time.fixedDeltaTime;

        _current = Mathf.Min(_current, _maximum);

        OnChange?.Invoke();
    }

    public void Load(PlayerSaveData data)
    {
        _current = data.Stamina;
        OnChange?.Invoke();
    }
}
