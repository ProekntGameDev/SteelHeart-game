using UnityEngine;

[ExecuteInEditMode]
public class SiblingIndex : MonoBehaviour
{
    [SerializeField] private int index;

    private void Awake()
    {
        transform.SetSiblingIndex(index);
    }
}