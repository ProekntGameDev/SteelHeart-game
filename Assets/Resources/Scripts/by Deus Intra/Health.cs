using UnityEngine;

public class Health : MonoBehaviour
{
    public delegate void VoidDelegate();

    public event VoidDelegate OnDeath;
    public event VoidDelegate OnChange;

    public float Maximum { get; private set; } = 100;
    public float Current { get; private set; } = 100;
    public bool IsFull { get { return Current == Maximum; } }
    
    public void Heal(float amount)
    {
        if (Current == Maximum) return;

        Current += amount;
        if (Current > Maximum) Current = Maximum;

        OnChange.Invoke();
    }

    public void FullHeal()
    {
        if (Current == Maximum) return;
        Current = Maximum;
        OnChange.Invoke();
    }

    public void Damage(float amount)
    {
        Current -= amount;
        if (Current <= 0)
        {
            Current = 0;
            OnDeath.Invoke();
        }
        OnChange.Invoke();
    }

    public float GetPercentage()
    {
        return Current / Maximum;
    }
}
