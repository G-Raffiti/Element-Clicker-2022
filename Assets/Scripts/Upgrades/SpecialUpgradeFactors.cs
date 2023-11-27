using System;
using System.Collections.Generic;
using System.Linq;
using _Extensions;
using BigNumbers;
using Buildings;
using Managers;
using Singletons;
using Trades;
using UnityEngine;

namespace Upgrades
{
    [CreateAssetMenu(fileName = "Production_Check_EventSender", menuName = "Scriptable Object/Production Check EventSender")]
    public class SpecialUpgradeFactors : ScriptableObjectSingleton<SpecialUpgradeFactors>
    {
        private Resource _lastStockAmount = new Resource();
        public static event Action<EResource> eOnBankValueChange;

        private Dictionary<Building, int> _lvl = new Dictionary<Building, int>();
        private Dictionary<Building, int> _transfoLvl = new Dictionary<Building, int>();
        public int TransfoLvl { get; private set; }
        public static event Action<int> eTotalBuildingLvl;
        public static event Action eTotalTransfoLvl;

        private Resource _lastDegenAmount = new Resource();
        public static event Action<EResource, bn> eOnDegenChangeValue;

        private void OnEnable()
        {
            Bank.eOnStockChange += SendResourcesEvents;
            Building.eOnLevelUp += SendTotalLevel;
            ProductionManager.eOnDegenUpdated += SendDegen;
        }

        private void SendTotalLevel(Building building)
        {
            _lvl.Set(building, building.Lvl);
            if(building.IsTransfo)
                _transfoLvl.Set(building,building.Lvl);

            int totalLvl = _lvl.Keys.Sum(build => _lvl[build]);
            TransfoLvl = _transfoLvl.Keys.Sum(build => _transfoLvl[build]);
            
            eTotalBuildingLvl?.Invoke(totalLvl);
            eTotalTransfoLvl?.Invoke();
        }

        
        private void SendResourcesEvents(Resource newStock)
        {
            foreach (EResource resource in newStock.Keys)
            {
                if (HasChanged(newStock[resource], _lastStockAmount[resource]))
                {
                    eOnBankValueChange?.Invoke(resource);
                    _lastStockAmount[resource] = newStock[resource];
                }
            }
        }
        
        private void SendDegen(Resource newDegen)
        {
            foreach (EResource resource in newDegen.Keys)
            {
                if(HasChanged(newDegen[resource], _lastDegenAmount[resource]))
                {
                    eOnDegenChangeValue?.Invoke(resource, newDegen[resource]);
                    _lastDegenAmount[resource] = newDegen[resource];
                }
            }
        }

        private static bool HasChanged(bn a, bn b)
        {
            if (a == b) return false;
            if (a.Exp != b.Exp) return true;
            if (bn.Abs(a - b) < new bn(1)) return false;
            if (Mathf.Abs(a.Value - b.Value) < 0.01) return false;
            return true;
        }
    }
}