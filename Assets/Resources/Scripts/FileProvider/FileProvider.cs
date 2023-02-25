using System;
using System.IO;

namespace SteelHeart
{
    public static class FilePath
    {
        public const string PATH_NOTES = "Assets/Json//Note/NoteMeta.json";
        public const string PATH_PLAYER = "Assets/Json//Player/PlayerMeta.json";
        public const string PATH_SETTINGS = "Assets/Json/Settings/SettingsMeta.json";
        
        public const string PATH_TRAP = "Assets/Json/Traps/TrapMeta.json";
        public const string PATH_UPGRADES = "Assets/Json/Upgrades/UpgradeMeta.json";
    }

    public static class FileProvider
    {
        private static void CheckPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(path,$"path cannot be null or empty");
            if (!File.Exists(path))
               File.Create(path);
        }

        public static void SaveInJson(string path, string content)
        {
            CheckPath(path);

            File.WriteAllText(path, content);
        }

        public static string GetJson(string path)
        {
            CheckPath(path);

            return File.ReadAllText(path);
        }
    }
}