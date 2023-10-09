using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using System.Linq;
using TMPro;

[CreateAssetMenu(fileName = "QTEButtonsUI", menuName = "Scriptable Objects/QTE UI")]
public class QTEButtons : ScriptableObject
{
    [SerializeField] private List<TMP_SpriteAsset> _icons;

    public void SetIconOrText(TextMeshProUGUI targetText, InputBinding binding)
    {
        string buttonName = binding.path.Split('/').Last().ToLower();
        TMP_SpriteAsset sprite = _icons.FirstOrDefault(x => x.name.ToLower() == buttonName);

        if (sprite == null)
            targetText.text = buttonName;
        else
        {
            targetText.spriteAsset = sprite;
            targetText.text = "<sprite=0>";
        }
    }
}
