using System;
using System.Collections.Generic;
using _Extensions;
using BigNumbers;
using Buildings;
using Trades;
using UnityEngine;
using UnityEngine.Serialization;

namespace Singletons
{
    [CreateAssetMenu(fileName = "Balance", menuName = "Scriptable Object/Singleton/Balance")]
    public class Balance : ScriptableObjectSingleton<Balance>
    {
        [Header("Bank")]
        [SerializeField] private float upgradeMaxStockFactor = 1.2f;
        public float UpgradeMaxStockFactor => upgradeMaxStockFactor;
        [SerializeField] private float upgradeMaxStockCost = 0.75f;
        public float UpgradeMaxStockCost => upgradeMaxStockCost;

        [SerializeField] private Resource.SResource _maxBankStock;
        public Resource.SResource StartMaxBankStock => _maxBankStock;
    
    
        [Header("Buildings")]
        [SerializeField] private float upgradeLvlFactor = 1.15f;
        public float UpgradeLvlFactor => upgradeLvlFactor;
        [SerializeField] private int levelPerPassivePoint = 10;
        public int LevelPerPassivePoint => levelPerPassivePoint;
        [SerializeField] private float upgradeLvlFactorOnStep = 5;
        public float UpgradeLvlFactorOnStep => upgradeLvlFactorOnStep;
        [SerializeField] private int levelPerStep = 25;
        public int LevelPerStep => levelPerStep;
        
        
        [Header("Button")] [SerializeField]
        private bn _timeOnClick = -0.2f;
        public bn TimeOnClick => _timeOnClick;
        
        [Header("Unlocks")] 
        [SerializeField] private List<BuildingUnlock> _buildingToUnlock;

        public List<BuildingUnlock> BuildingToUnlock => _buildingToUnlock;


        [Serializable]
        public struct BuildingUnlock
        {
            public BuildingSO building;
            public EResource resource;
            public bn value;
        }
    }
}