using UnityEngine;

public class MedKit : MonoBehaviour, IInteractableMonoBehaviour
{
    public float restorationAmount = 20;

    public void Interact(Transform obj)
    {
        var health = obj.GetComponent<Health>();
        if (health == null) return;

        if (health.IsFull == false)
        {
            health.Heal(restorationAmount);
            gameObject.SetActive(false);
        }        
    }
}
