using System;
using BigNumbers;
using Buildings;
using Singletons;
using Trades;
using UnityEngine;

namespace Upgrades
{
    [CreateAssetMenu(fileName = "_Upgrade_Transfo_Total", menuName = "Scriptable Object/Upgrades/TransfoLvl")]
    public class UpgradeSOTransfoLvl : UpgradeSOSpecial
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
                prod[resource].Added *= (float) lvl * RunStats.Instance.TownLvl;
                prod[resource].Multi = bn.Max(1, prod[resource].Multi * lvl * RunStats.Instance.TownLvl);
                prod[resource].Increased *= (float) lvl * RunStats.Instance.TownLvl;
            }

            return prod;
        }
    }
}