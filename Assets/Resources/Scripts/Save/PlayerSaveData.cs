using System;
using UnityEngine;

[Serializable]
public class PlayerSaveData
{
    public float Health { get; private set; }
    public float Stamina { get; private set; }
    public int Gears { get; private set; }
    public PlayerMovementData AdditionalData { get; private set; }

    public PlayerSaveData(Player player, PlayerMovementData additionalData = null)
    {
        Health = player.Health.Current;
        Stamina = player.Stamina.Current;
        Gears = player.GearsHolder.Gears;

        AdditionalData = additionalData;
    }

    public void Load(Player player)
    {
        player.Health.Load(this);
        player.Stamina.Load(this);
        player.GearsHolder.Load(this);
    }
}

[Serializable]
public class PlayerMovementData
{
    public Quaternion CameraRotation { get; private set; }
    public Quaternion Rotation { get; private set; }
    public Vector3 RelativePosition { get; private set; }
    public Vector3 Velocity { get; private set; }

    public PlayerMovementData(Player player, Vector3 relativePosition, Camera camera)
    {
        CameraRotation = camera.transform.rotation;
        Rotation = player.transform.rotation;
        RelativePosition = relativePosition;
        Velocity = player.Movement.CharacterController.CurrentVelocity + player.Movement.CharacterController.VerticalVelocity * Vector3.up;
    }

    public void Load(Player player, Vector3 relativeToWorld, Camera camera)
    {
        player.Movement.CharacterController.SetPosition(relativeToWorld + RelativePosition);
        player.transform.rotation = Rotation;

        player.Movement.CharacterController.CurrentVelocity = new Vector3(Velocity.x, 0, Velocity.z);
        player.Movement.CharacterController.VerticalVelocity = Velocity.y;

        camera.transform.rotation = CameraRotation;
    }
}
