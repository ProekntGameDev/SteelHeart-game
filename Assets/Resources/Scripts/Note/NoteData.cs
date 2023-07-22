using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Note", fileName = "NoteData")]
public class NoteData : ScriptableObject
{
    public string Header => _header;
    public string Subheader => _subheader;
    public NoteContent NoteContent => _noteContent;

    [SerializeField] private string _header;
    [SerializeField] private string _subheader;
    [SerializeField] private NoteContent _noteContent;
}
