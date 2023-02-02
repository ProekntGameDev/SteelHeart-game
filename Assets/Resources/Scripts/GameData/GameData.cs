using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace SteelHeart
{
    public static class GameData
    {
        public static NoteData Note = new NoteData();

        public class NoteData
        {
            public List<GameMeta.NoteData> CollectedNotes = new List<GameMeta.NoteData>();

            public GameMeta.NoteData GetNoteById(int id) =>
                CollectedNotes.FirstOrDefault(n => n.Id == id);
            
            public GameMeta.NoteData GetNoteByTitle(string title) => 
                CollectedNotes.FirstOrDefault(n => n.Title == title);

            public void Initialize()
            {
                string json = FileProvider.GetJson(FilePath.PATH_NOTES);
                CollectedNotes = JsonHelper.DeserializeObject<List<GameMeta.NoteData>>(json);
            }
        }
    }
}