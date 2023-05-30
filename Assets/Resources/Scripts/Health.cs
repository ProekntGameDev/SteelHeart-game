using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IDamagable
{
    [HideInInspector] public UnityEvent OnDeath;
    [HideInInspector] public UnityEvent<float> OnChange;

    public float Current => _currentHealth;
    public float Max => _maximumHealth;

    [SerializeField] private float _maximumHealth = 100;

    private float _currentHealth;
    
    public void Heal(float value)
    {
        if (value < 0)
            throw new System.ArgumentOutOfRangeException(nameof(value));

        if (_currentHealth == _maximumHealth) 
            return;

        _currentHealth = Mathf.Min(_currentHealth + value, _maximumHealth);

        OnChange?.Invoke(_currentHealth);
    }

    public void TakeDamage(float damage)
    {
        if (damage < 0)
            throw new System.ArgumentOutOfRangeException(nameof(damage));

        _currentHealth = Mathf.Max(_currentHealth - damage, 0);
        if (_currentHealth <= 0)
            OnDeath?.Invoke();

        OnChange?.Invoke(_currentHealth);
    }

    private void Awake()
    {
        _currentHealth = _maximumHealth;
    }
}
