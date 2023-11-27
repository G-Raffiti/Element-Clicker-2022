using System;
using BigNumbers;
using Singletons;

namespace _SaveSystem.SerializableClasses
{
    [Serializable]
    public struct SettingsSerializable
    {
        public EBigNumberFormat Format;
        public ELvlUpMode LvlUpMode;
        public float Tick;
        public string BuildVersion;
        public SettingsSerializable(Settings instance)
        {
            Format = instance.Format;
            LvlUpMode = instance.LvlUpMode;
            Tick = instance.Tick;
            BuildVersion = instance.BuildVersion;
        }
    }
}