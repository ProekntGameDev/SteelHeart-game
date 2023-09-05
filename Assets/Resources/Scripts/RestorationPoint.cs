using UnityEngine;
using Interfaces;

public class RestorationPoint : MonoBehaviour, IInteractableMonoBehaviour
{
    public void Interact(Transform obj)
    {
        var health = obj.GetComponent<Health>();
        if (health == null) 
            return;

        health.Heal(health.Max);
    }
}
