using SaveSystem;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private string _saveFolder;
    private UnitySaver _saver;

    public void Save(PlayerSaveData data, string name = "save") => _saver.Save(data, name);

    public PlayerSaveData Load(string name = "save") => _saver.Load<PlayerSaveData>(name);

    public SaveData<PlayerSaveData>[] GetSaves() => _saver.GetSaves<PlayerSaveData>();

    public void Delete(string name = "save") => File.Delete(Path.Combine(_saveFolder, name));

    private void Awake()
    {
        _saver = new UnitySaver();
        _saveFolder = Path.Combine(Application.persistentDataPath, "Saves");
    }
}
