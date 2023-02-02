using System.Collections.Generic;
using System.Linq;

namespace SteelHeart
{
    public static class GameData
    {
        public static NoteData Note = new NoteData();
        public static PlayerData Player = new PlayerData();
        public static SettingsData Settings = new SettingsData();

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
        
        public class PlayerData
        {
            public GameMeta.PlayerData Meta { get; private set; }
            public GameMeta.Settings.PlayerSettings Settings { get; private set; }

            public void Initialize()
            {
                string json = FileProvider.GetJson(FilePath.PATH_PLAYER);
                Meta = JsonHelper.DeserializeObject<GameMeta.PlayerData>(json);
                Settings = GameData.Settings.Meta.PlayerSettingsMeta;
            }
        }

        public class SettingsData
        {
            public GameMeta.Settings Meta { get; private set; }

            public void Initialize()
            {
                string json = FileProvider.GetJson(FilePath.PATH_SETTINGS);
                Meta = JsonHelper.DeserializeObject<GameMeta.Settings>(json);
            }
        }
    }
}