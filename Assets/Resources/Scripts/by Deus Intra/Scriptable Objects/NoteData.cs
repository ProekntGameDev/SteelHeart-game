using UnityEngine;

[CreateAssetMenu(fileName = "Level Data", menuName = "Scriptable Objects/Note Data", order = 1)]
public class NoteData : ScriptableObject
{
    public string title;
    [TextArea]
    public string text;
}
