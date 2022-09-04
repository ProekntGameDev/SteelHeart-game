using UnityEngine;

public class NightvisionUpgrade : MonoBehaviour, ITriggerableMonoBehaviour
{
    public void Trigger(Transform obj)
    {
        var nightvisionAbility = obj.GetComponent<PlayerNightvisionAbility>();
        if (nightvisionAbility == null) return;

        nightvisionAbility.enabled = true;
        gameObject.SetActive(false);
    }
}
