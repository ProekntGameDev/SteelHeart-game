using System;
using SteelHeart;
using UnityEngine;

namespace NoteEditor
{
    public class Note
    {
        public readonly int Id;
        public readonly string Title;
        public readonly string Message;

        public Note(int id, string title, string message)
        {
            Id = id;
            Title = title;
            Message = message;
        }
    }

    public static class NoteEditorTools
    {
        public static bool IsTitleValid(string title) => 
            !string.IsNullOrEmpty(title);

        public static bool IsMessageValid(string message) =>
            !string.IsNullOrEmpty(message);
        
        public static bool IsIdValid(string id) => 
            !string.IsNullOrEmpty(id) || Int32.TryParse(id, out int parsed);

        public static bool IsDataValid(string id, string title, string message)
        {
            return IsIdValid(id) && IsTitleValid(title) && IsMessageValid(message);
        }

        public static void Redact(GameMeta.NoteData oldData, Note newData)
        {
            var oldNote = JsonHelper.SerializeObject(oldData);
            var newNote = JsonHelper.SerializeObject(newData);
            FileProvider.ReplaceInJson(FilePath.PATH_NOTES, oldNote, newNote);
        }
        
        public static bool Save(Note note)
        {
            var response = JsonHelper.SerializeObject(note);
            FileProvider.AppendInJson(FilePath.PATH_NOTES, response);
            GameData.Note.Initialize();

            return response != null;
        }
        
        public static bool TrySave(string id, string title, string message)
        {
            if (IsDataValid(id, title, message))
            {
                int parsedId = Convert.ToInt32(id);
                var noteData = GameData.Note.GetNoteById(parsedId);

                if (noteData == null)
                {
                    var note = new Note(parsedId, title, message);
                    Save(note);
                }
                else
                {
                    Debug.LogError($"Note with ID: {parsedId} already exist");
                }

                return noteData == null || noteData.Id != parsedId;
            }

            return false;
        }
    }
}