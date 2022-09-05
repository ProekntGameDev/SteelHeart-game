using UnityEngine;

public class JetpackUpgrade : MonoBehaviour, ITriggerableMonoBehaviour
{
    public void Trigger(Transform obj)
    {
        var jumpController = obj.GetComponent<PlayerJumpController>();
        if (jumpController == null) return;

        jumpController.maxJetpackJumps++;
        gameObject.SetActive(false);
    }
}
