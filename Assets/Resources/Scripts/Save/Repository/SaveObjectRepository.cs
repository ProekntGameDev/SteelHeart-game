using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaveSystem
{
    public class SaveObjectRepository : Repository
    {
        public override bool IsChange { get; set; }

        public Dictionary<int, bool> IsUseDictionary = new Dictionary<int, bool>();

        private const string Key = "PlayerSaveObjectData";

        public override void Load()
        {
            GetValuesSaveObjectData(SaveService.LoadData<SaveObjectData>(Key));
        }

        public override void Save()
        {
            if (IsChange)
                SaveService.SaveData<SaveObjectData>(Key, SetValuesSaveObjectData());
        }

        private SaveObjectData SetValuesSaveObjectData()
        {
            SaveObjectData saveObjectData = new SaveObjectData();
            saveObjectData.IsUseDictionary = IsUseDictionary;

            return saveObjectData;
        }

        private void GetValuesSaveObjectData(SaveObjectData saveObjectData)
        {
            IsUseDictionary = saveObjectData.IsUseDictionary;
        }
    }

    [Serializable]
    public class SaveObjectData
    {
        public Dictionary<int, bool> IsUseDictionary;
    }
}
