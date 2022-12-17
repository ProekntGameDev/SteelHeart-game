using System;
using UnityEngine;
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
            if (value < 0)
            {
                Debug.LogWarning("health has dropped below zero. Health is changed to 0");
                value = 0;
            }
            else if (value > MaxValue)
            {
                Debug.LogWarning(
                    $"health has increased MaxValue ({MaxValue}). the health was lowered to MaxValue ({MaxValue})");
                value = MaxValue;
            }

            OnHealthChanged?.Invoke(value);
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

    public void Kill()
    {
        HealthValue = 0;
    }

    public void Resurrect()
    {
        HealthValue = MaxValue;
    }


    private static void ThrIfLt0(float value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value));
    }
}