using Buildings;
using Trades;
using UnityEngine;

namespace Upgrades
{
    [CreateAssetMenu(fileName = "BUILDING_ID_Upgrade_ProdOnClick_NAME", menuName = "Scriptable Object/Upgrades/ProdOnClick")]
    public class UpgradeSOProductionOnClick : UpgradeSO
    {
        [SerializeField] private Production.SProductions _onClick;
        public Production Production => new Production(_onClick);
        
        public override void OnUpgrade(Building building, int upgradeLvl)
        {
            base.OnUpgrade(building, upgradeLvl);
            building.RegisterClickProductionUpgrade(this);
        }
    }
}