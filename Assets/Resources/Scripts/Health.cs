using UnityEngine;

public class Health : MonoBehaviour
{
    public delegate void VoidDelegate();

    public event VoidDelegate OnDeath;
    public event VoidDelegate OnChange;

    [SerializeField] private float _maximum = 100;
    [SerializeField] private float _current = 100;

    public float Maximum { get { return _maximum; } }
    public float Current { get { return _current; } }
    public float Percentage { get { return _current / _maximum; } }
    public bool IsFull { get { return _current == _maximum; } }
    
    public void Heal(float amount)
    {
        if (_current == _maximum) return;

        _current += amount;
        if (_current > _maximum) _current = _maximum;

        OnChange?.Invoke();
    }

    public void FullHeal()
    {
        if (_current == _maximum) return;
        _current = _maximum;
        OnChange?.Invoke();
    }

    public void Damage(float amount)
    {
        _current -= amount;
        if (_current <= 0)
        {
            _current = 0;
            OnDeath?.Invoke();
        }
        OnChange?.Invoke();
    }
}
