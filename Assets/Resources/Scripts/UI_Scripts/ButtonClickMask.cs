using UnityEngine;
using UnityEngine.UI;

public class ButtonClickMask : MonoBehaviour
{
    [Range(0f, 1f)] public float AlphaLevel = 1f;
    private Image _buttonImage;
    
    private void Start()
    {
        // _buttonImage = gameObject.GetComponentInChildren<Image>();
    }

    private void Update()
    {
        _buttonImage = gameObject.GetComponentInChildren<Image>();
        _buttonImage.alphaHitTestMinimumThreshold = AlphaLevel;
    }
}