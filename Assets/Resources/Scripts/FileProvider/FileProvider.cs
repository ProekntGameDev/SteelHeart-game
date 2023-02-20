using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace SteelHeart
{
    public static class FilePath
    {
        public const string PATH_NOTES = "Assets/Json//Note/NoteJson.json";
        public const string PATH_PLAYER = "Assets/Json//Player/PlayerJson.json";
        public const string PATH_SETTINGS = "Assets/Json/Settings/SettingsJson.json";
        public const string PATH_TRAPS = "Assets/Json/Trap/TrapJson.json";
    }

    public static class FileProvider
    {
        public static void CheckPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException($"path cannot be null or empty");
            if (!File.Exists(path))
                throw new ArgumentNullException($"File {path} does not exist");
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