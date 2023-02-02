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
            public int Id;
            public string Title;
            public string Message;
        }

        [Serializable]
        public class PlayerData
        {
            public float WalkSpeed;
            public float SprintSpeed;
            public float SprintStaminaSpend;
            public float FloorCheckRayLength;
            public float JumpForce;
            public float JumpTimeOut;
            public float SlideForce;
            public float MaxSlideTime;
            public float MaxStamina;
            public float SufficientStamina;
            public float StaminaRestorationRate;
            public float MaxHealth;

            
        }

        [Serializable]
        public class Settings
        {
            public PlayerSettings PlayerSettingsMeta = new PlayerSettings();
            
            [Serializable]
            public class PlayerSettings
            {
                public KeyCode SprintKey = KeyCode.LeftShift;
                public KeyCode ClimbUpwardsKey = KeyCode.W;
                public KeyCode ClimbDownwardsKey = KeyCode.S;
                public KeyCode CrouchKey = KeyCode.S;
                public KeyCode InteractionKey = KeyCode.E;
                public KeyCode SlidingKey = KeyCode.LeftControl;
                public KeyCode JumpKey = KeyCode.Space;
                public KeyCode BlockKey = KeyCode.Mouse1;
                public KeyCode GrappleKey = KeyCode.Mouse0;
                public KeyCode UngrappleKey = KeyCode.Space;
                public KeyCode NightVisionKey = KeyCode.N;
                public KeyCode ShootKey = KeyCode.Mouse0;
                public KeyCode SlowMotionKey = KeyCode.T;
            }
        }
    }
}
