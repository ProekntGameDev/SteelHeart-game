using System.Collections.Generic;

namespace SteelHeart
{
    public static class MetaManager
    {
        public static T Load<T>(string path)
        {
            var json = FileProvider.GetJson(path);
            return JsonHelper.DeserializeObject<T>(json);
        }

        public static void Save(object obj, string path)
        {
            var content = JsonHelper.SerializeObject(obj);
            FileProvider.SaveInJson(path, content);
        }

        public static void Save<T>(object objToSave, string path)
        {
            var tempData = Load<List<T>>(path) ?? new List<T>();
            tempData.Add((T)objToSave);
            var content = JsonHelper.SerializeObject(tempData);
            FileProvider.SaveInJson(path, content);
        }
    }
}
