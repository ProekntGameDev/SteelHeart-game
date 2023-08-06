using System;
using UnityEngine;

[Serializable]
public class PlayerSaveData
{
    public float Health { get; private set; }
    public float Stamina { get; private set; }
    public int Gears { get; private set; }
    public Vector3 Position { get; private set; }

    public PlayerSaveData(Player player)
    {
        Health = player.Health.Current;
        Stamina = player.Stamina.Current;
        Gears = player.GearsHolder.Gears;
        Position = player.transform.position;
    }
}
