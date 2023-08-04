using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaveSystem
{
    public class SaveObjectInteractor : Interactor<SaveObjectRepository>
    {
        public override Repository Repository => base.Repository;
        protected override SaveObjectRepository Data => base.Data;

        public void SetSaveObjectIsUse(int index, bool isUse)
        {
            Repository.IsChange = true;
            Data.IsUseDictionary[index] = isUse;
        }

        public bool GetSaveObjectIsUse(int index)
        {
            if(Data.IsUseDictionary.ContainsKey(index))
                return Data.IsUseDictionary[index];
            return false;
        }
    }
}