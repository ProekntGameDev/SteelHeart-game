using UnityEngine;

public class HookUpgrade : MonoBehaviour, ITriggerableMonoBehaviour
{
    public void Trigger(Transform obj)
    {
        var grapplingAbility = obj.GetComponent<PlayerGrapplingAbility>();
        if (grapplingAbility == null) return;

        grapplingAbility.isHookUse_Allow = true;
        gameObject.SetActive(false);
    }
}
