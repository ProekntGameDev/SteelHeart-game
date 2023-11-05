using System;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IDamagable
{
    [HideInInspector] public UnityEvent OnDeath;
    [HideInInspector] public UnityEvent<Damage> OnTakeDamage;
    [HideInInspector] public UnityEvent<float> OnChange;

    public float Current => _currentHealth;
    public float Max => _maximumHealth;

    [SerializeField] private float _maximumHealth = 100;

    private float _currentHealth;
    
    public void Heal(float value)
    {
        if (enabled == false)
            return;

        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value));

        if (_currentHealth == _maximumHealth) 
            return;

        _currentHealth = Mathf.Min(_currentHealth + value, _maximumHealth);

        OnChange?.Invoke(_currentHealth);
    }

    public DamageBlockResult TakeDamage(Damage damage, Health from)
    {
        DamageBlockResult result = DamageBlockResult.None;

        if (enabled == false)
            return result;

        if (damage.CanBlock)
        {
            result = TryBlockDamage(damage, from);
            if (result == DamageBlockResult.Blocked || result == DamageBlockResult.Reflected)
                return result;
        }

        _currentHealth = Mathf.Max(_currentHealth - damage.Value, 0);
        if (_currentHealth <= 0)
            OnDeath?.Invoke();

        OnChange?.Invoke(_currentHealth);

        if(_currentHealth > 0)
            OnTakeDamage?.Invoke(damage);

        return result;
    }

    public void Load(PlayerSaveData data)
    {
        _currentHealth = data.Health;
        OnChange?.Invoke(_currentHealth);
    }

    protected virtual DamageBlockResult TryBlockDamage(Damage damage, Health from) => DamageBlockResult.None;

    private void Awake()
    {
        if(_currentHealth == 0)
            _currentHealth = _maximumHealth;
    }
}
