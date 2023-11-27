using System;
using System.Collections.Generic;
using _SaveSystem.SerializableClasses;

namespace _SaveSystem.Data
{
    [Serializable]
    public class GameData
    {
        public StatSerializable Run;
        public StatSerializable Total;
        public ResourceSerializable BankStock;
        public ResourceSerializable BankMaxStock;
        public List<BuildingSerializable> Buildings;
        public long SaveTime;
        public SettingsSerializable Settings;
        public long RunTime;
    }
}