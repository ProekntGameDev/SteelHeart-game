using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour, IInteractableMonoBehaviour
{
    [SerializeField] private string text;
    public static List<string> notes;

    private void Awake()
    {
        if (notes == null) notes = new List<string>();
    }

    public void Interact(Transform obj)
    {
        if (obj.GetComponent<PlayerController>() == null) return;

        Debug.Log("Note: " + text);
        notes.Add(text);
        gameObject.SetActive(false);
    }
}
