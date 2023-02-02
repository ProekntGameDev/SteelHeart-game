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
    }
}
