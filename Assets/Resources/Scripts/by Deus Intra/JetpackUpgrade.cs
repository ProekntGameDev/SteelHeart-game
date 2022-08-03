using UnityEngine;

public class JetpackUpgrade : MonoBehaviour, IInteractableMonoBehaviour
{
    public void Interact(Transform obj)
    {
        var jumpController = obj.GetComponent<JumpController>();
        if (jumpController == null) return;

        jumpController.maxJetpackJumps++;
        gameObject.SetActive(false);
    }
}
