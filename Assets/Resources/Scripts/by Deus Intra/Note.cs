using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour, ITriggerableMonoBehaviour
{
    [SerializeField] private string text;
    public static List<string> notes;

    private void Awake()
    {
        if (notes == null) notes = new List<string>();
    }

    public void Trigger(Transform obj)
    {
        if (obj.GetComponent<PlayerMovementController>() == null) return;

        Debug.Log("Note: " + text);
        notes.Add(text);
        gameObject.SetActive(false);
    }
}
