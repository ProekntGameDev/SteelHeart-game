using System.Collections.Generic;
using System.Linq;

namespace SteelHeart
{
    public static class GameData
    {
        public static readonly NoteData Note = new NoteData();
        public static readonly PlayerData Player = new PlayerData();
        public static readonly SettingsData Settings = new SettingsData();
        public static readonly TrapsData Traps = new TrapsData();
        public static readonly UpgradesData Upgrades = new UpgradesData();

        public class NoteData
        {
            public List<GameMeta.Note> Notes = new List<GameMeta.Note>();

            public GameMeta.Note GetNoteById(int id) =>
                Notes.FirstOrDefault(n => n.id == id);
            
            public GameMeta.Note GetNoteByTitle(string title) => 
                Notes.FirstOrDefault(n => n.title == title);

            public void Initialize()
            {
                Notes = MetaManager.Load<List<GameMeta.Note>>(FilePath.PATH_NOTES);
            }
        }
        
        public class PlayerData
        {
            public GameMeta.Player Meta { get; private set; }
            public GameMeta.Settings.PlayerSettings Settings { get; private set; }

            public void Initialize()
            {
                Meta = MetaManager.Load<GameMeta.Player>(FilePath.PATH_PLAYER);
                Settings = GameData.Settings.Meta.playerSettingsMeta;
            }
        }

        public class SettingsData
        {
            public GameMeta.Settings Meta { get; private set; }

            public void Initialize()
            {
                Meta = MetaManager.Load<GameMeta.Settings>(FilePath.PATH_SETTINGS);
            }
        }

        public class TrapsData
        {
            public List<GameMeta.Trap> Traps { get; private set; }

            public GameMeta.Trap GetTrapById(int id) =>
                Traps.FirstOrDefault(t => t.id == id);
            
            public void Initialize()
            {
                Traps = MetaManager.Load<List<GameMeta.Trap>>(FilePath.PATH_TRAP);
            }
        }

        public class UpgradesData
        {
            public List<GameMeta.Upgrade> Upgrades { get; private set; }

            public void Initialize()
            {
                Upgrades = MetaManager.Load<List<GameMeta.Upgrade>>(FilePath.PATH_UPGRADES);
            }
        }
    }
}