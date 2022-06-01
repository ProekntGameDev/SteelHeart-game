using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpecification : MonoBehaviour
{
    [SerializeField] private string text;
    public void AddInJournal() { NotesJournal.notes.Add(text); Debug.Log(text); }
}
