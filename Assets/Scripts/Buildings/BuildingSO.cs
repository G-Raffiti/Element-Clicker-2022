using System.Collections.Generic;
using _Extensions;
using BigNumbers;
using Trades;
using Trees;
using UnityEditor;
using UnityEngine;
using Upgrades;

namespace Buildings
{
    [CreateAssetMenu(fileName = "Building_", menuName = "Scriptable Object/Building")]
    public class BuildingSO : ScriptableObject
    {
        // Name
        [SerializeField] private string buildingName;
        public string BuildingName => buildingName;
    
        // Building Icon
        [SerializeField] private Sprite icon;
        public Sprite Icon => icon;
        
        // Initial Cost of the Building
        [SerializeField] private bool hasCostSteps;
        public bool HasCostSteps => hasCostSteps;
        [SerializeField] private Resource.SResource baseCost;
        public Resource.SResource BaseCost => baseCost;
    
        // Initial Production of the Building
        [NonReorderable][SerializeField] private Production.SProductions baseProduction;
        public Production.SProductions BaseProduction => baseProduction;
    
        // Upgrade Tree of the Building
        [SerializeField] private BuildingTreeSO tree;
        public BuildingTreeSO Tree => tree;
        
        // Upgrade that modify the building (not part of the skillTree)
        [SerializeField] private List<UpgradeSO> baseUpgrades;
        public List<UpgradeSO> BaseUpgrades => baseUpgrades;
        
        // Passive Points at Lvl 0
        [SerializeField] private int passivePoint;
        public int BasePassivePoint => passivePoint;
    }
}