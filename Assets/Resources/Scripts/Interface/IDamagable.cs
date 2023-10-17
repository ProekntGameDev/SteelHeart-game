using UnityEngine;
using System;

[Serializable]
public struct Damage
{
    public Damage(float value, bool canBlock, bool isPiercing)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value));

        _value = value;
        _canBlock = canBlock;
        _isPiercing = isPiercing;
    }

    public float Value => _value;
    public bool CanBlock => _canBlock;
    public bool IsPiercing => _isPiercing;

    public static explicit operator Damage(float value) => new Damage(value, true, false);

    [SerializeField] private float _value;
    [SerializeField] private bool _canBlock;
    [SerializeField] private bool _isPiercing;
}

public enum DamageBlockResult
{
    None,
    Blocked,
    Reflected,
    Broken
}

public interface IDamagable
{
    DamageBlockResult TakeDamage(Damage damage, Health from);
}
