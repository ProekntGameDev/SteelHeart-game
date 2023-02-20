using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SteelHeart
{
    namespace JsonTest
    {
        public class JsonTest : MonoBehaviour
        {
            private void Awake()
            {
                GameData.Settings.Initialize();
                GameData.Player.Initialize();
                GameData.Traps.Initialize();
            }
        }
    }
}