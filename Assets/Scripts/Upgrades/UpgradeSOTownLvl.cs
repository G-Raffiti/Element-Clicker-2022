using System;
using Buildings;
using Singletons;
using Trades;
using UnityEngine;

namespace Upgrades
{
    [CreateAssetMenu(fileName = "BUILDING_ID_Upgrade_TownLvl_NAME", menuName = "Scriptable Object/Upgrades/TownLvl")]
    public class UpgradeSOTownLvl : UpgradeSOSpecial
    {
        [SerializeField] private Production.SProductions _productionForEachTownLvl;
        private void OnEnable()
        {
            Building.eOnLevelUp += SendRatio;
        }

        private void SendRatio(Building building)
        {
            SendRatio();
        }

        public override Production GetRatio(int lvl)
        {
            Production prod = new Production(_productionForEachTownLvl);
            foreach (EResource resource in Enum.GetValues(typeof(EResource)))
            {
                prod[resource].Increased *= (float) lvl * RunStats.Instance.TownLvl;
            }

            return prod;
        }
    }
}