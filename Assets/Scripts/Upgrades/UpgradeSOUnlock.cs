using System;
using Buildings;
using UnityEngine;

namespace Upgrades
{
    [CreateAssetMenu(fileName = "BUILDING_ID_Upgrade_Unlock_NAME", menuName = "Scriptable Object/Upgrades/Unlock")]
    public class UpgradeSOUnlock : UpgradeSO
    {
        [Header("Unlock Upgrade")] 
        [SerializeField] private BuildingSO buildingType;
        
        public static event Action<BuildingSO> eUnlockBuilding;
        
        public override void OnUpgrade(Building building, int upgradeLvl)
        {
            eUnlockBuilding?.Invoke(buildingType);
        }
    }
}