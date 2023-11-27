using System;
using BigNumbers;
using Singletons;

namespace _SaveSystem.SerializableClasses
{
    [Serializable]
    public class StatSerializable
    {
        public ResourceSerializable Produced;
        public bn Clicks;
        public bn TownLvl;
        
        public StatSerializable(RunStats instance)
        {
            Produced = new ResourceSerializable(instance.Produced);
            Clicks = new bn(instance.Clicks);
            TownLvl = instance.TownLvl;
        }

        public StatSerializable(TotalStats instance)
        {
            Produced = new ResourceSerializable(instance.Produced);
            Clicks = new bn(instance.Clicks);
            TownLvl = new bn(instance.TownLvl);
        }
    }
}