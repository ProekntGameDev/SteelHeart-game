using System;
using System.Collections.Generic;
using UnityEngine;

namespace SteelHeart
{
    public class GameMeta
    {
        [Serializable]
        public class Note
        {
            public int id;
            public string title;
            public string message;
            public bool isCollected;
        }

        [Serializable]
        public class Player
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

            [NonSerialized] 
            public List<Upgrade> Upgrades = GameData.Upgrades.Upgrades?.FindAll(u => u.isOpen);

            [NonSerialized] 
            public List<Note> Notes = GameData.Note.Notes?.FindAll(n => n.isCollected);
        }

        [Serializable]
        public class Settings
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
        
        //Traps
        [Serializable]
        public class Trap
        {
            public int id;
        }

        [Serializable]
        public class Spikes : Trap
        {
            public float damage;
            public float moveSpeedUp;
            public float moveSpeedDown;
            public float height;
        }

        [Serializable]
        public class Mine : Trap
        {
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
        public class ExplosionStretch : Trap
        {
            public float laserLength;
            public float explosionRadius;
            public float damage;
            public float damageMultiplier;
            public float radiusMultiplier;
        }

        [Serializable]
        public class SelfDestroyingPlatform : Trap
        {
            public float destroyingTimeInMs;
        }

        [Serializable]
        public class CircularSaw : Trap
        {
            public float startDamage;
            public float maxDamage;
            public float damageMultiplier;
        }

        [Serializable]
        public class Turret : Trap
        {
            public float shootRadius;
            public float delayPerShoot;
            public float health;
            public float damage;
            public float deactivationDuration;
        }

        [Serializable]
        public class Spring : Trap
        {
            public float activationDuration;
            public float repulsionDistance;
        }

        [Serializable]
        public class RoboSpider : Trap
        {
            public float damage;
            public float health;
            public float deactivationDuration;
            public float activationRadius;
        }

        [Serializable]
        public class RoboTrap : Trap
        {
            public float health;
            public float enemyProbability;
            public float currencyProbability;
            public float damage;
            public int currencyAmount;
        }

        [Serializable]
        public class Upgrade
        {
            public int id;
            public bool isOpen = false;
        }
        
        //Upgrades
        [Serializable]
        public class Jetpack : Upgrade
        {
            public int jumpsCount;
            public float jumpCoolDown;
            public float jumpHeight;
        }
        
        [Serializable]
        public class GrappleHook : Upgrade
        {
            public float hookHeight;
            public float maxDistance;
            public float displacementSpeed;
        }
        
        [Serializable]
        public class Shield : Upgrade
        {
            public float cooldown;
            public float duration;
            public float firstStageDuration;
            public float firstStageBlock;
            public float secondStageDuration;
            public float secondStageBlock;
            public float thirdStageDuration;
            public float thirdStageBlock;
        }

        [Serializable]
        public class NightVision : Upgrade
        {
            public float spidersBacklightTime;
            public float springBacklightTime;
            public float backlightAlpha;
        }

        [Serializable]
        public class BasicGun : Upgrade
        {
            public float damage;
            public float bulletSpeed;
            public int maxCapacity;
            public float shootDelay;
        }
        
        [Serializable]
        public class GrenadeGun : Upgrade
        {
            public float explosionRadius;
            public int maxCapacity;
            public float bulletSpeed;
            public float shootDelay;
        }
        
        [Serializable]
        public class ShrinkingGun : Upgrade
        {
            public float actionTime;
            public float cooldown;
        }
    }
}