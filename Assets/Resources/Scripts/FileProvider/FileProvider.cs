using System;
using System.IO;
using System.Text;

namespace SteelHeart
{
    public static class FilePath
    {
        public const string PATH_NOTES = "Assets/Resources/Scripts/JsonHelper/Notes.json";
    }
    
    public static class FileProvider
    {
        public static void ReplaceInJson(string path, string oldContent, string newContent)
        {
            var json = File.ReadAllText(path);
            json = json.Replace(oldContent, newContent);
            File.WriteAllText(path, json);
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