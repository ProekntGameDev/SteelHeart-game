using UnityEngine;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class PlayerRepository : Repository
{
    public override bool IsChange { get; set; }

    public float HealthPlayer;
    public int GearsPlayer;
    public float[] PositionPlayer = new float[3];

    private const string Key = "PlayerSave";

    public override void Load()
    {
        GetValuesPlayerData(SaveService.LoadData<PlayerData>(Key));
    }

    public override void Save()
    {
        SaveService.SaveData<PlayerData>(Key, SetValuesPlayerData());
    }

    private PlayerData SetValuesPlayerData()
    {
        PlayerData playerdata = new PlayerData();
        playerdata.HealthPlayer = HealthPlayer;
        playerdata.GearsPlayer = GearsPlayer;
        playerdata.PositionPlayer = PositionPlayer;

        return playerdata;
    }

    private void GetValuesPlayerData(PlayerData playerData)
    {
        HealthPlayer = playerData.HealthPlayer;
        GearsPlayer = playerData.GearsPlayer;
        PositionPlayer = playerData.PositionPlayer;
    }
}

[Serializable]
public class PlayerData
{
    public float HealthPlayer;
    public int GearsPlayer;
    public float[] PositionPlayer = new float[3];
}
