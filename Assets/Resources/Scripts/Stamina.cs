using UnityEngine;

public class Stamina : MonoBehaviour
{
    public delegate void VoidDelegate();
    public event VoidDelegate OnChange;

    [SerializeField] private float _maximum = 10;
    [Tooltip("Minimal amount to perform action")]
    [SerializeField] private float _sufficient = 5f;
    [SerializeField] private float _restorationRate = 1f;
    
    private float _current;

    public float Maximum { get { return _maximum; } }
    public float Current { get { return _current; } }
    public float Percentage { get { return _current / _maximum; } }

    public bool IsSufficient => _current > _sufficient;


    private void Start()
    {
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
}
