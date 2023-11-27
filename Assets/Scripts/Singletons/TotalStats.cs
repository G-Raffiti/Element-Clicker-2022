using System;
using System.Collections.Generic;
using _Extensions;
using _SaveSystem.Data;
using _SaveSystem.SerializableClasses;
using BigNumbers;
using Buildings;
using Trades;
using UnityEngine;

namespace Singletons
{
    [CreateAssetMenu(fileName = "TotalStats", menuName = "Scriptable Object/Singleton/TotalStats")]
    public class TotalStats : ScriptableObjectSingleton<TotalStats>
    {
        public Resource Produced { get; private set; } = new Resource();
        public bn Clicks { get; private set; } = 0;
        private Dictionary<BuildingSO, int> townLvl = new Dictionary<BuildingSO, int>();
        public bn TownLvl { get; private set; } = 0;

        private void OnEnable()
        {
            Bank.eOnProduce += OnProduced;
            Building.eOnLevelUp += OnLevelUp;
            MainBtn.eOnClick += OnClick;
        }

        private void OnClick()
        {
            Clicks++;
        }

        private void OnLevelUp(Building obj)
        {
            if (!townLvl.ContainsKey(obj.BuildingSO))
            {
                townLvl.Set(obj.BuildingSO, 0);
            }
            TownLvl += obj.Lvl - townLvl[obj.BuildingSO];
            townLvl.Set(obj.BuildingSO, obj.Lvl);
        }

        private void OnProduced(Resource produced)
        {
            Produced += produced;
        }
        
        public void LoadData(GameData data)
        {
            Clicks = data.Run.Clicks;
            TownLvl = data.Run.TownLvl;
            Produced = data.Run.Produced.GetResource();

            townLvl = new Dictionary<BuildingSO, int>();
            foreach (BuildingSerializable building in data.Buildings)
            {
                townLvl.Set(building.buildingSO, building.lvl);
            }
        }
    }
}