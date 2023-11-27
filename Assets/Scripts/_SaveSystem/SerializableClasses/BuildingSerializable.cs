using System;
using Buildings;
using Upgrades;

namespace _SaveSystem.SerializableClasses
{
    [Serializable]
    public struct BuildingSerializable
    {
        public BuildingSO buildingSO;
        public int lvl;
        public ResourceSerializable cost;
        public int passivePoint;
        public DictionarySerializable<UpgradeSO, int> actualUpgrades;
        public bool IsActive;

        public BuildingSerializable(Building building, bool isActive)
        {
            buildingSO = building.BuildingSO;
            lvl = building.Lvl;
            cost = new ResourceSerializable(building.Cost);
            passivePoint = building.PassivePoint;
            actualUpgrades = new DictionarySerializable<UpgradeSO, int>(building.ActualUpgradesLvl);
            IsActive = isActive;
        }
    }
}