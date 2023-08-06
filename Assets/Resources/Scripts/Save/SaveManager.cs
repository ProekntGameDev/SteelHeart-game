using SaveSystem;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private UnitySaver _saver;

    public PlayerSaveData Load(string name)
    {
        return _saver.Load<PlayerSaveData>(name);
    }

    public void Save(string name, Player player)
    {
        PlayerSaveData playerSaveData = new PlayerSaveData(player);
        _saver.Save(playerSaveData, name);
    }

    public SaveData<PlayerSaveData>[] GetSaves() => _saver.GetSaves<PlayerSaveData>();

    private void Awake()
    {
        _saver = new UnitySaver();
    }
}
