using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public static class SaveService
{
    public static T LoadData<T>(string key) where T : new()
    {
        key = $"/{key}.save";

        if (File.Exists(Application.persistentDataPath + key))
        {
            BinaryFormatter _binaryFormatter = new BinaryFormatter();
            FileStream _file = File.Open(Application.persistentDataPath + key, FileMode.Open);
            var savedData = (T)_binaryFormatter.Deserialize(_file);
            _file.Close();
            Debug.Log($"Data uploaded: {typeof(T)}");
            return savedData;
        }
        else
        {
            Debug.Log($"No data saved: {typeof(T)}");
        }
        return new T();
    }

    public static void SaveData<T>(string key, T data)
    {
        if (data == null) return;

        key = $"/{key}.save";

        BinaryFormatter _binaryFormatter = new BinaryFormatter();
        FileStream _file = File.Create(Application.persistentDataPath + key);
        _binaryFormatter.Serialize(_file, data);
        _file.Close();
        Debug.Log($"Data saved: {typeof(T)}");
    }
}
