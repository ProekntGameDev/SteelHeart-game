using SaveSystem;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private UnitySaver _saver;

    public void Save(PlayerSaveData data, string name = "save") => _saver.Save(data, name);

    public PlayerSaveData Load(string name = "save") => _saver.Load<PlayerSaveData>(name);

    public SaveData<PlayerSaveData>[] GetSaves() => _saver.GetSaves<PlayerSaveData>();

    private void Awake()
    {
        _saver = new UnitySaver();
    }
}
