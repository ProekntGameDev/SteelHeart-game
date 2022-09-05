using System.Collections.Generic;

public static class StaticGameData
{
    private static List<NoteData> _collectedNotes;

    public static bool AddNote(NoteData note)
    {
        if (_collectedNotes == null) _collectedNotes = new List<NoteData>();

        if (_collectedNotes.Contains(note)) return false;
        
        _collectedNotes.Add(note);
        return true;

    }

    public static List<NoteData> GetNotes()
    {
        if (_collectedNotes == null) _collectedNotes = new List<NoteData>();
        return _collectedNotes;
    }
}
