using Buildings;
using UnityEngine;

namespace Upgrades
{
    [CreateAssetMenu(fileName = "BUILDING_ID_Upgrade_ToClick_NAME", menuName = "Scriptable Object/Upgrades/ToClick")]
    public class UpgradeSOToClick : UpgradeSO
    {
        [SerializeField] private float _ratio;
        public float Ratio => _ratio;

        public override void OnUpgrade(Building building, int upgradeLvl)
        {
            building.RegisterToClickUpgrade(this);
        }
    }
}