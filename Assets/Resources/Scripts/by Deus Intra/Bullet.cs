using UnityEngine;

public class Bullet : MonoBehaviour, IInteractableMonoBehaviour
{
    public float damage;

    public void Interact(Transform obj)
    {
        var health = obj.GetComponent<Health>();
        if (health == null) return;

        health.Damage(damage);
        gameObject.SetActive(false);
    }
}
