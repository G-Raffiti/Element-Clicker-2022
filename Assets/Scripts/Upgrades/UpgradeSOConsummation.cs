using System;
using System.Collections.Generic;
using BigNumbers;
using Trades;
using UnityEngine;

namespace Upgrades
{
    [CreateAssetMenu(fileName = "_Upgrade_Consum_Total", menuName = "Scriptable Object/Upgrades/ConsumTest")]
    public class UpgradeSOConsummation : UpgradeSOSpecial
    {
        [SerializeField] private List<EResource> _resourceTest;
        private Production off = new Production();
        private Production on = new Production();
        private void OnEnable()
        {
            foreach (EResource resource in Enum.GetValues(typeof(EResource)))
            {
                off[resource].Multi = 0;
            }
        }

        public override Production GetRatio(int lvl)
        {
            foreach (EResource resource in _resourceTest)
            {
                if (Bank.Stock[resource] < new bn(5, 1)) return off;
            }
            return on;
        }
    }
}