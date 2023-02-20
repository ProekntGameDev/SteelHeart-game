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

        public class NoteData
        {
            public List<GameMeta.NoteData> Notes = new List<GameMeta.NoteData>();

            public GameMeta.NoteData GetNoteById(int id) =>
                Notes.FirstOrDefault(n => n.id == id);
            
            public GameMeta.NoteData GetNoteByTitle(string title) => 
                Notes.FirstOrDefault(n => n.title == title);

            public void Initialize()
            {
                Notes = GameMeta.Load<List<GameMeta.NoteData>>(FilePath.PATH_NOTES);
            }
        }
        
        public class PlayerData
        {
            public GameMeta.PlayerMeta Meta { get; private set; }
            public GameMeta.SettingsMeta.PlayerSettings Settings { get; private set; }

            public void Initialize()
            {
                Meta = GameMeta.Load<GameMeta.PlayerMeta>(FilePath.PATH_PLAYER);
                Settings = GameData.Settings.Meta.playerSettingsMeta;
            }
        }

        public class SettingsData
        {
            public GameMeta.SettingsMeta Meta { get; private set; }

            public void Initialize()
            {
                Meta = GameMeta.Load<GameMeta.SettingsMeta>(FilePath.PATH_SETTINGS);
            }
        }

        public class TrapsData
        {
            public List<GameMeta.Spikes> Spikes { get; private set; }
            public List<GameMeta.Mine> Mines { get; private set; }
            public List<GameMeta.ExplosionStretch> ExplosionStretches { get; private set; }
            public List<GameMeta.SelfDestroyingPlatform> SelfDestroyingPlatforms { get;private set; }
            public List<GameMeta.Spring> Springs { get; private set; }
            public List<GameMeta.RoboTrap> RoboTraps { get; private set; }
            public List<GameMeta.RoboSpider> RoboSpiders { get; private set; }
            
            public void Initialize()
            {
                Spikes = GameMeta.Load<List<GameMeta.Spikes>>(FilePath.PATH_TRAPS);
            }
            
        }
    }
}