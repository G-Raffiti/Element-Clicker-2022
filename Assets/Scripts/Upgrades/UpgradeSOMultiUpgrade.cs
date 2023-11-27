using System.Collections.Generic;
using _Extensions;
using Buildings;
using UnityEngine;

namespace Upgrades
{
    [CreateAssetMenu(fileName = "_Upgrade_Multi_", menuName = "Scriptable Object/Upgrades/Multi")]
    public class UpgradeSOMultiUpgrade : UpgradeSO
    {
        [SerializeField] private List<UpgradeSO> _upgrades;

        public override void OnUpgrade(Building building, int upgradeLvl)
        {
            base.OnUpgrade(building, upgradeLvl);
            foreach (UpgradeSO upgrade in _upgrades)
            {
                building.ActualUpgradesLvl.Set(upgrade, upgradeLvl);
                upgrade.OnUpgrade(building,upgradeLvl);
            }
        }
    }
}