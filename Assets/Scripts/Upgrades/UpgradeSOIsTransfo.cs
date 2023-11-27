using Buildings;
using UnityEngine;

namespace Upgrades
{
    [CreateAssetMenu(fileName = "BUILDING_ID_Upgrade_Transfo_NAME", menuName = "Scriptable Object/Upgrades/Transfo")]
    public class UpgradeSOIsTransfo : UpgradeSO
    {
        public override void OnUpgrade(Building building, int upgradeLvl)
        {
            building.IsTransfo = true;
        }
    }
}