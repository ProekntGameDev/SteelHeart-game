using UnityEngine;

public class HookUpgrade : MonoBehaviour, IInteractableMonoBehaviour
{
    public void Interact(Transform obj)
    {
        var grapplingGun = obj.GetComponent<GrapplingGun>();
        if (grapplingGun == null) return;

        grapplingGun.isHookUse_Allow = true;
        gameObject.SetActive(false);
    }
}
