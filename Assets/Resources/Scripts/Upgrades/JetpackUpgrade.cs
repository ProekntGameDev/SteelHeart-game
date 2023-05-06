using UnityEngine;
using Interfaces;

public class JetpackUpgrade : MonoBehaviour, ITriggerableMonoBehaviour
{
    public void Trigger(Transform obj)
    {
        //TODO: Rework for new movement code
        //var jumpController = obj.GetComponent<PlayerJump>();
        //if (jumpController == null) return;

        //jumpController.maxJetpackJumps++;
        //gameObject.SetActive(false);
    }
}
