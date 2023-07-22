using UnityEngine;
using NaughtyAttributes;

[System.Serializable]
public class NoteContent
{
    public string Text => _text;
    public AudioClip Audio => _audio;

    [SerializeField, AllowNesting] private AudioClip _audio;

    [ResizableTextArea]
    [SerializeField, AllowNesting] private string _text;
}
