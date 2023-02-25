using SteelHeart;
using UnityEngine;

namespace NoteEditor
{
    public class Note
    {
        public readonly int Id;
        public readonly string Title;
        public readonly string Message;
        public readonly bool IsCollected;
        
        public Note(int id, string title, string message)
        {
            Id = id;
            Title = title;
            Message = message;
            IsCollected = GameData.Note.GetNoteById(id)?.isCollected ?? false;
        }
    }

    public static class NoteEditorTools
    {
        public static bool IsTitleValid(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                Debug.LogError($"TITLE CANNOT BE EMPTY");
                return false;
            }

            return true;
        }

        public static bool IsMessageValid(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                Debug.LogError($"MESSAGE CANNOT BE EMPTY");
                return false;
            }

            return true;
        }

        public static bool IsIdValid(int id)
        {
            var note = GameData.Note.GetNoteById(id);
            if (note != null)
            {
                if (id == note.id)
                {
                    Debug.LogError($"ID: {id} ALREADY EXIST");
                    return false;
                }
            }
            
            return true;
        }

        public static bool IsNoteValid(int id, string title, string message) =>
             IsIdValid(id) && IsTitleValid(title) && IsMessageValid(message);

        private static void Save(Note note)
        {
            MetaManager.Save<GameMeta.Note>(note, FilePath.PATH_NOTES);
            GameData.Note.Initialize();
            
        }

        public static void Redact(Note note)
        {
             Save(note);
        }
        
        public static bool TrySave(Note note)
        {
            if (IsNoteValid(note.Id, note.Title, note.Message))
            {
                var noteData = GameData.Note.GetNoteById(note.Id);

                if (noteData == null)
                {
                    Save(note);
                }
                else
                {
                    Debug.LogError($"Note with ID: {note.Id} already exist");
                }

                return noteData == null;
            }

            return false;
        }
    }
}