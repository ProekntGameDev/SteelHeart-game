using System;
using UnityEngine.Events;

public class Health
{
    public readonly float MaxValue;

    // not visible in the inspector 'cause it's readonly
    public readonly UnityEvent<float> OnHealthChanged = new UnityEvent<float>();

    private float _healthValue;

    public Health(float maxValue = 1)
    {
        MaxValue = maxValue;
        _healthValue = MaxValue;
    }

    private float HealthValue
    {
        get => _healthValue;
        set
        {
            ThrIfNotBetween0AndMaxValue(value);

            OnHealthChanged.Invoke(value);
            _healthValue = value;
        }
    }


    public void Heal(float value)
    {
        ThrIfLt0(value);
        HealthValue += value;
    }

    public void Damage(float value)
    {
        ThrIfLt0(value);
        HealthValue -= value;
    }


    private void ThrIfNotBetween0AndMaxValue(float value)
    {
        if (value > MaxValue || value < 0)
            throw new ArgumentOutOfRangeException(nameof(value));
    }

    private static void ThrIfLt0(float value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value));
    }
}