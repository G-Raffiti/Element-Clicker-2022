using Buildings;
using Trades;
using UnityEngine;

namespace Upgrades
{
    [CreateAssetMenu(fileName = "BUILDING_ID_Upgrade_Production_NAME", menuName = "Scriptable Object/Upgrades/Production")]
    public class UpgradeSOProduction : UpgradeSO
    {
        [Header("Production Upgrade")] 
        [SerializeField] private Production.SProductions _productions;
        public Production Production => new Production(_productions);

        public override void OnUpgrade(Building building, int upgradeLvl)
        {
            building.RegisterProductionUpgrade(this);
        }
    }
}