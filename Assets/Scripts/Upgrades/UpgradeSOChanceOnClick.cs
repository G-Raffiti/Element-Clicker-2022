using System;
using System.Collections.Generic;
using Buildings;
using Trades;
using UnityEngine;

namespace Upgrades
{
    [CreateAssetMenu(fileName = "_Upgrade_Chance_", menuName = "Scriptable Object/Upgrades/Chance")]
    public class UpgradeSOChanceOnClick : UpgradeSO
    {
        [SerializeField] private int value;
        [SerializeField] private List<EResource> resource;

        public static event Action<EResource, int> eChanceValue;

        public override void OnUpgrade(Building building, int upgradeLvl)
        {
            base.OnUpgrade(building, upgradeLvl);
            foreach (EResource eResource in resource)
            {
                eChanceValue?.Invoke(eResource, value);
            }
        }
    }
}