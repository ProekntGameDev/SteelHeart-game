using UnityEngine;
using System;

namespace SaveSystem
{
    public class PlayerInteractor : Interactor<PlayerRepository>
    {
        public static Action<float> OnChangePlayerHealth;
        public static Action<int> OnChangePlayerGears;

        public override Repository Repository => base.Repository;
        protected override PlayerRepository Data => base.Data;

        public void ChangeHealthPlayer(float value)
        {
            Repository.IsChange = true;
            float health = Data.HealthPlayer + value;
            if (health < 0f) health = 0f;
            else if (health > 1f) health = 1f;
            Data.HealthPlayer = health;

            OnChangePlayerHealth?.Invoke(GetHealthPlayer());
        }

        public float GetHealthPlayer()
        {
            return Data.HealthPlayer;
        }

        public void ChangeGearsPlayer(int value)
        {
            Repository.IsChange = true;
            int gears = Data.GearsPlayer + value;
            if (gears < 0) gears = 0;
            Data.GearsPlayer = gears;

            OnChangePlayerGears?.Invoke(GetGearsPlayer());
        }

        public int GetGearsPlayer()
        {
            return Data.GearsPlayer;
        }

        public void SetPositionPlayer(Vector3 position)
        {
            Repository.IsChange = true;
            for (int i = 0; i < Data.PositionPlayer.Length; i++)
                Data.PositionPlayer[i] = position[i];
        }

        public Vector3 GetPositionPlayer()
        {
            Vector3 position = new Vector3(0, 0, 0);

            for (int i = 0; i < Data.PositionPlayer.Length; i++)
                position[i] = Data.PositionPlayer[i];

            return position;
        }

    }
}
