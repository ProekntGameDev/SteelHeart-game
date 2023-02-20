using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Serialization;

namespace SteelHeart
{
    public class GameMeta
    {
        [Serializable]
        public class NoteData
        {
            public int id;
            public string title;
            public string message;
            public bool isCollected;
        }

        [Serializable]
        public class PlayerMeta
        {
            public float walkSpeed;
            public float sprintSpeed;
            public float sprintStaminaSpend;
            public float floorCheckRayLength;
            public float jumpForce;
            public float jumpTimeOut;
            public float slideForce;
            public float maxSlideTime;
            public float maxStamina;
            public float sufficientStamina;
            public float staminaRestorationRate;
            public float maxHealth;
        }

        [Serializable]
        public class SettingsMeta
        {
            public PlayerSettings playerSettingsMeta = new PlayerSettings();

            [Serializable]
            public class PlayerSettings
            {
                public KeyCode sprintKey = KeyCode.LeftShift;
                public KeyCode climbUpwardsKey = KeyCode.W;
                public KeyCode climbDownwardsKey = KeyCode.S;
                public KeyCode crouchKey = KeyCode.S;
                public KeyCode interactionKey = KeyCode.E;
                public KeyCode slidingKey = KeyCode.LeftControl;
                public KeyCode jumpKey = KeyCode.Space;
                public KeyCode blockKey = KeyCode.Mouse1;
                public KeyCode grappleKey = KeyCode.Mouse0;
                public KeyCode ungrappleKey = KeyCode.Space;
                public KeyCode nightVisionKey = KeyCode.N;
                public KeyCode shootKey = KeyCode.Mouse0;
                public KeyCode slowMotionKey = KeyCode.T;
            }
        }

        [Serializable]
        public class Spikes
        {
            public int id;
            public float damage;
            public float moveSpeedUp;
            public float moveSpeedDown;
            public float height;
        }

        [Serializable]
        public class Mine
        {
            public int id;
            public float activationRadius;
            public float explosionRadius;
            public float activationDuration;
            public float damage;
            public float damageMultiplier;
            public float radiusMultiplier;
            public float moveSpeedUp;
            public float moveSpeedDown;
            public float height;
        }

        [Serializable]
        public class ExplosionStretch
        {
            public int id;
            public float laserLength;
            public float explosionRadius;
            public float damage;
            public float damageMultiplier;
            public float radiusMultiplier;
        }

        [Serializable]
        public class SelfDestroyingPlatform
        {
            public int id;
            public float destroyingTimeInMs;
        }

        [Serializable]
        public class CircularSaw
        {
            public int id;
            public float startDamage;
            public float maxDamage;
            public float damageMultiplier;
        }

        [Serializable]
        public class Turret
        {
            public int id;
            public float shootRadius;
            public float delayPerShoot;
            public float health;
            public float damage;
            public float deactivationDuration;
        }

        [Serializable]
        public class Spring
        {
            public int id;
            public float activationDuration;
            public float repulsionDistance;
        }

        [Serializable]
        public class RoboSpider
        {
            public int id;
            public float damage;
            public float health;
            public float deactivationDuration;
            public float activationRadius;
        }

        [Serializable]
        public class RoboTrap
        {
            public int id;
            public float health;
            public float enemyProbability;
            public float currencyProbability;
            public float damage;
            public int currencyAmount;
        }

        [Serializable]
        public class GrappleHookUpgrade
        {
            public float maxDistance;
            public float displacementSpeed;
        }

        [Serializable]
        public class ShieldUpgrade
        {
            public float blockStaminaSpend;
            public float duration;
        }

        public static T Load<T>(string path)
        {
            var json = FileProvider.GetJson(path);
            return JsonHelper.DeserializeObject<T>(json);
        }

        public static void Save(object obj, string file)
        {
            var content = JsonHelper.SerializeObject(obj);
            FileProvider.SaveInJson(file, content);
        }

        public static void Save<T>(T objToSave, string file)
        {
            var tempData = Load<List<T>>(file) ?? new List<T>();
            tempData.Add(objToSave);

            var content = JsonHelper.SerializeObject(tempData);
            FileProvider.SaveInJson(file, content);
        }
    }
}