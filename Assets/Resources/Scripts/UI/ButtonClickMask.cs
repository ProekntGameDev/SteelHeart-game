using UnityEngine;
using UnityEngine.UI;

public class ButtonClickMask : MonoBehaviour
{
    [SerializeField] [Range(0f, 1f)] private float _alphaLevel = 1f;

    private void Start()
    {
        SetMask();
    }

    public void SetMask()
    {
        var buttonImage = gameObject.GetComponentInChildren<Image>();
        buttonImage.alphaHitTestMinimumThreshold = _alphaLevel;
    }
}