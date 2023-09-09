using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent OnInteract;

    public bool CanInteract { get; set; }

    [SerializeField, Layer] private int _outlineLayer = 8;
    [SerializeField] private bool _setChildrenLayer;

    private int _defaultLayer;

    public virtual void OnSelect() => SetLayerRecursive(gameObject, _outlineLayer);
    public virtual void OnUnselect() => SetLayerRecursive(gameObject, _defaultLayer);

    public virtual void Interact() => OnInteract?.Invoke();

    private void Start()
    {
        _defaultLayer = gameObject.layer;
    }

    private void SetLayerRecursive(GameObject gameObject, int index)
    {
        gameObject.layer = index;

        if (_setChildrenLayer == false)
            return;

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            SetLayerRecursive(gameObject.transform.GetChild(i).gameObject, index);
        }
    }

    public void OnEnable() => CanInteract = true;
    public void OnDisable() => CanInteract = false;
}
