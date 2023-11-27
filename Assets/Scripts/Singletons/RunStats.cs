using System;
using System.Collections.Generic;
using _Extensions;
using _SaveSystem.Data;
using _SaveSystem.SerializableClasses;
using BigNumbers;
using Buildings;
using Newtonsoft.Json;
using Trades;
using UnityEngine;

namespace Singletons
{
    [CreateAssetMenu(fileName = "Stats", menuName = "Scriptable Object/Singleton/Stats")]
    public class RunStats : ScriptableObjectSingleton<RunStats>
    {
        public Resource Produced { get; private set; } = new Resource();
        private List<Building> townLvl = new List<Building>();
        public int TownLvl { get; private set; } = 0;
        public int TranfoLvl { get; private set; } = 0;
        public bn Clicks { get; private set; } = 0;
        public DateTime StartRun { get; private set; }

        private void OnEnable()
        {
            StartRun = DateTime.Now;
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
            if(!townLvl.Contains(obj)) 
                townLvl.Add(obj);
            TownLvl = 0;
            TranfoLvl = 0;
            foreach (Building building in townLvl)
            {
                TownLvl += building.Lvl;
                if (building.IsTransfo)
                    TranfoLvl += building.Lvl;
            }
        }

        private void OnProduced(Resource produced)
        {
            Produced += produced;
        }

        public void LoadData(GameData data)
        {
            Clicks = data.Run.Clicks;
            Produced = data.Run.Produced.GetResource();

            townLvl = new List<Building>();

            StartRun = DateTime.FromBinary(data.RunTime);
        }
    }
}