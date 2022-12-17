using UnityEngine;
using Interfaces;

public class RestorationPoint : MonoBehaviour, IInteractableMonoBehaviour
{
    public void Interact(Transform obj)
    {
        var health = obj.GetComponent<HealthOld>();
        if (health == null) return;

        health.FullHeal();
    }
}
