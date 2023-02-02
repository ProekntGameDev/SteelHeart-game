using System;
using System.IO;
using System.Text;

namespace SteelHeart
{
    public static class FilePath
    {
        public const string PATH_NOTES = "Assets/Json/Note/NoteJson.json";
        public const string PATH_PLAYER = "Assets/Json/Player/PlayerJson.json";
        public const string PATH_SETTINGS = "Assets/Json/Settings/SettingsJson.json";
    }
    
    public static class FileProvider
    {
        public static void ReplaceInJson(string path, string oldContent, string newContent)
        {
            var json = File.ReadAllText(path);
            json = json.Replace(oldContent, newContent);
            File.WriteAllText(path, json);
        }

        public static void SaveInJson(string path, string content)
        {
            File.WriteAllText(path, content);
        }
        
        public static void AppendInJson(string path, string content)
        {
            var json = File.ReadAllText(path);
            content = json.Length == 2 ? content : $",{content}";
            var newJson = json.Insert(json.IndexOf(']'), content);
            File.WriteAllText(path, newJson);
        }

        public static string GetJson(string path)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
                return null;

            return File.ReadAllText(path);
        }
    }
}