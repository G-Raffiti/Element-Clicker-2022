using System;
using System.Collections.Generic;
using _Extensions;
using _SaveSystem.SerializableClasses;
using BigNumbers;
using Singletons;
using Trades;
using Trees;
using UnityEngine;
using Upgrades;

namespace Buildings
{
    public class Building
    {
        // public Properties
        public BuildingSO BuildingSO { get; private set; }
        public Resource ProductionTotal { get; private set; } = new Resource();
        public Resource OnClickTotal { get; private set; } = new Resource();
        public Resource Cost { get; private set; } = new Resource();
        public int Lvl { get; private set; } = 0;
        public int PassivePoint { get; private set; } = 0;
        public bool IsTransfo { get; set; } = false;
        public Dictionary<UpgradeSO, int> ActualUpgradesLvl { get; private set; } = new Dictionary<UpgradeSO, int>();
        public bool HasCostSteps { get; private set; }
        
        // fields that handles parts of the production 
        private Production _onClick = new Production();
        private Production _production = new Production();
        private float _percentProductionToClick = 0;

        // Lists of the Upgrades in different Types
        private List<UpgradeSOProduction> _productionUps = new List<UpgradeSOProduction>();
        private Dictionary<UpgradeSOSpecial, Production> _specialUps = new Dictionary<UpgradeSOSpecial, Production>();
        private List<UpgradeSOConvert> _convertUps = new List<UpgradeSOConvert>();
        private Dictionary<UpgradeSOProductionOnClick, Production> _onClickUps =
            new Dictionary<UpgradeSOProductionOnClick, Production>();

        // Events
        public static event Action<Building, Resource> eOnProductionChanged;
        public static event Action<Building, Resource> eOnClickProductionChanged;
        public static event Action<Building, int> eOnPassivePointChange;
        public static event Action<Building> eOnLevelUp;
        public event Action<Resource,Resource> eUpdate;

        //Constructor
        public Building(BuildingSO buildingSO)
        {
            BuildingSO = buildingSO;
            _production = new Production(buildingSO.BaseProduction);
            Cost = new Resource(buildingSO.BaseCost);
            PassivePoint = buildingSO.BaseUpgrades.Count + buildingSO.BasePassivePoint;
            HasCostSteps = buildingSO.HasCostSteps;
            foreach (UpgradeSO upgradeSo in buildingSO.BaseUpgrades)
            {
                Upgrade(upgradeSo);
            }
        }

        /// <summary>
        /// Try to Level Up "lvlNumber" at the time
        /// </summary>
        public void LevelUp(int lvlNumber)
        {
            Resource cost = new Resource(BuildingBtn.GetCostFor(HasCostSteps, Lvl, lvlNumber, Cost));
            
            if (!Bank.Pay(cost)) return;
            
            for (int i = 0; i < lvlNumber; i++)
            {
                Lvl += 1;
                Cost = SetNewCost(HasCostSteps, Lvl, Cost);
                if (Lvl % Balance.Instance.LevelPerPassivePoint == 0)
                {
                    PassivePoint += 1;
                    eOnPassivePointChange?.Invoke(this, PassivePoint);
                }
                foreach (EResource resource in Enum.GetValues(typeof(EResource)))
                {
                    _production[resource].Increased += 1;
                    _onClick[resource].Increased += 1;
                }
            }
            
            ProductionTotal = new Resource(_production);
            
            eOnLevelUp?.Invoke(this);
            UpdateProduction();
        }

        /// <summary>
        /// Set New cost with the Balance Factors
        /// </summary>
        public static Resource SetNewCost(bool costSteps, int lvl, Resource actualCost)
        {
            Resource cost = new Resource(actualCost);
            if (costSteps && lvl % Balance.Instance.LevelPerStep == 0)
                cost *= Balance.Instance.UpgradeLvlFactorOnStep;
            else
                cost *= Balance.Instance.UpgradeLvlFactor;
            if (cost[EResource.Click] > 500) cost[EResource.Click]= 100;
            return cost;
        }

        /// <summary>
        /// Try to Buy and Add a new Upgrade to the Building
        /// </summary>
        public void Upgrade(UpgradeSO upgradeSO)
        {
            if (!ActualUpgradesLvl.ContainsKey(upgradeSO))
            {
                ActualUpgradesLvl.Add(upgradeSO, 0);
            }
            if (ActualUpgradesLvl[upgradeSO] >= upgradeSO.MaxLvl) return;
            
            ActualUpgradesLvl[upgradeSO]++;

            Cost += upgradeSO.AddedCost * Mathf.Pow(10, ActualUpgradesLvl[upgradeSO] - 1);
            foreach (EResource resource in Bank.BasicResources)
            {
                Cost[resource] = bn.Max(0, Cost[resource]);
            }
            PassivePoint -= ActualUpgradesLvl[upgradeSO];
            
            upgradeSO.OnUpgrade(this, ActualUpgradesLvl[upgradeSO]);
            
            eOnPassivePointChange?.Invoke(this, PassivePoint);
        }
    
        /// <summary>
        /// Update the Production of the Building
        /// </summary>
        private void UpdateProduction()
        {
            Production variant = new Production();
            Production variantOnClick = new Production();
            foreach (UpgradeSOSpecial variation in _specialUps.Keys)
            {
                variant.Merge(_specialUps[variation]);
                variantOnClick.Merge(_specialUps[variation]);
            }

            variant.Merge(_production);
            variantOnClick.Merge(_onClick);

            Resource updatedProduction = new Resource(variant);
            Resource updatedOnClick = new Resource(variantOnClick);
            
            foreach (UpgradeSOConvert convert in _convertUps)
            {
                updatedProduction = convert.Convert(updatedProduction, ActualUpgradesLvl[convert]);
                updatedOnClick = convert.Convert(updatedOnClick, ActualUpgradesLvl[convert]);
            }
            
            updatedOnClick += updatedProduction * _percentProductionToClick;
            updatedProduction *= (1 - _percentProductionToClick);

            if (Totem.IsActive)
            {
                updatedProduction[EResource.Air] = 0;
            }
            eUpdate?.Invoke(updatedProduction,updatedOnClick);
            eOnProductionChanged?.Invoke(this, updatedProduction);
            eOnClickProductionChanged?.Invoke(this, updatedOnClick);
            ProductionTotal = new Resource(updatedProduction);
            OnClickTotal = new Resource(updatedOnClick);
        }
        
        /// <summary>
        /// Change the Ratio of one Special Upgrade then Update the Production
        /// </summary>
        public void UpdateProductionVariation(UpgradeSOSpecial upgrade, Production production)
        {
            _specialUps.Set(upgrade, production);
            UpdateProduction();
        }
        
        /// <summary>
        /// Add a new UpgradeSOProduction to this Building then Update the Production
        /// </summary>
        public void RegisterProductionUpgrade(UpgradeSOProduction upgrade)
        {
            if(!ActualUpgradesLvl.ContainsKey(upgrade)) return;
            if(!_productionUps.Contains(upgrade)) _productionUps.Add(upgrade);
            
            // Base Production
            _production = new Production(BuildingSO.BaseProduction);
        
            // Lvl of the Building
            foreach (EResource resource in _production.Keys)
            {
                _production[resource].Increased += Lvl;
            }
            
            // Upgrades
            foreach (UpgradeSOProduction upgradeSO in _productionUps)
            {
                //merge once per upgrade lvl
                for (int i = 0; i < ActualUpgradesLvl[upgradeSO]; i++)
                {
                    _production.Merge(upgradeSO.Production);
                }
            }
        
            // Save Updated Production
            UpdateProduction();
        }

        /// <summary>
        /// Add a new UpgradeSOToClick to this Building then Update the Production
        /// </summary>
        public void RegisterToClickUpgrade(UpgradeSOToClick upgradeSoToClick)
        {
            _percentProductionToClick = Mathf.Clamp01(ActualUpgradesLvl[upgradeSoToClick] * upgradeSoToClick.Ratio);
            UpdateProduction();
        }

        /// <summary>
        /// Add a new UpgradeSOConvert to this Building then Update the Production
        /// </summary>
        public void RegisterConvertUpgrade(UpgradeSOConvert upgradeSOConvert)
        {
            if(_convertUps.Contains(upgradeSOConvert)) return;
            _convertUps.Add(upgradeSOConvert);
        }

        /// <summary>
        /// Add a new UpgradeSOProductionOnClick to this Building then Update the Production
        /// </summary>
        public void RegisterClickProductionUpgrade(UpgradeSOProductionOnClick upgradeSoToClick)
        {
            if(!_onClickUps.ContainsKey(upgradeSoToClick))
                _onClickUps.Set(upgradeSoToClick, upgradeSoToClick.Production);

            Production upgradeProd = upgradeSoToClick.Production;
            for (int i = 1; i < ActualUpgradesLvl[upgradeSoToClick]; i++)
            {
                upgradeProd.Merge(upgradeSoToClick.Production);
            }
            _onClickUps.Set(upgradeSoToClick, upgradeProd);
            
            _onClick = new Production();
            foreach (EResource resource in Enum.GetValues(typeof(EResource)))
            {
                _onClick[resource].Increased += Lvl;
            }
            
            foreach (UpgradeSOProductionOnClick upgrade in _onClickUps.Keys)
            {
                _onClick.Merge(_onClickUps[upgrade]);
            }

            UpdateProduction();
        }
        
        /// <summary>
        /// Gain one Level given by the Clovers
        /// </summary>
        public void FreeLevelUp(BuildingSO building)
        {
            if(building != BuildingSO) return;
            
            
            Lvl += 1;
            
            if (Lvl % Balance.Instance.LevelPerPassivePoint == 0)
            {
                PassivePoint += 1;
                eOnPassivePointChange?.Invoke(this, PassivePoint);
            }
            foreach (EResource resource in Enum.GetValues(typeof(EResource)))
            {
                _production[resource].Increased += 1;
                _onClick[resource].Increased += 1;
            }
            
            ProductionTotal = new Resource(_production);
                
            eOnLevelUp?.Invoke(this);
            UpdateProduction();
        }

        /// <summary>
        /// Set the Data to the precedent save
        /// </summary>
        public void LoadData(BuildingSerializable building)
        {
            Cost = building.cost.GetResource();
            ActualUpgradesLvl = new Dictionary<UpgradeSO, int>(building.actualUpgrades);
            Lvl = building.lvl;
            PassivePoint = building.passivePoint;
            
            foreach (UpgradeSO upgrade in ActualUpgradesLvl.Keys)
            {
                upgrade.OnUpgrade(this, ActualUpgradesLvl[upgrade]);
            }
            
            
            // Base Production
            _production = new Production(BuildingSO.BaseProduction);
        
            // Lvl of the Building
            foreach (EResource resource in _production.Keys)
            {
                _production[resource].Increased += Lvl;
            }
            
            // Upgrades
            foreach (UpgradeSOProduction upgradeSO in _productionUps)
            {
                //merge once per upgrade lvl
                for (int i = 0; i < ActualUpgradesLvl[upgradeSO]; i++)
                {
                    _production.Merge(upgradeSO.Production);
                }
            }
        
            // Save Updated Production
            UpdateProduction();
            
            eUpdate?.Invoke(ProductionTotal, OnClickTotal);
            eOnPassivePointChange?.Invoke(this, PassivePoint);
            eOnLevelUp?.Invoke(this);
            eOnProductionChanged?.Invoke(this, ProductionTotal);
            eOnClickProductionChanged?.Invoke(this, OnClickTotal);
        }
    }
}