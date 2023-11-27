using System;
using Singletons;
using TMPro;
using Trades;
using Trees;
using UnityEngine;
using UnityEngine.UI;

namespace Buildings
{
    public class BuildingBtn : MonoBehaviour
    {
        //UI
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _nameTxt;
        [SerializeField] private TextMeshProUGUI _lvlTxt;
        [SerializeField] private TextMeshProUGUI _nextLvlTxt;
        [SerializeField] private TextMeshProUGUI _costTxt;
        [SerializeField] private TextMeshProUGUI _productionTxt;
        [SerializeField] private TextMeshProUGUI _passivePoint;
        [SerializeField] private Image _cost;
        [SerializeField] private Color enable;
        [SerializeField] private Color disable;
        [SerializeField] private Image _treeButton;

        //Number of Level Up Depending on the Settings
        private int _lvlNumber;
        
        // Building
        public Building Building { get; private set; }
        
        public static event Action<BuildingSO> eOpenTree;

        private Resource GetActualCost()
        {
            Resource cost = Building.Cost;
            
            switch (Settings.Instance.LvlUpMode)
            {
                case ELvlUpMode.round:
                    _lvlNumber = 10 - (Building.Lvl % 10);
                    return GetCostFor(Building.HasCostSteps, Building.Lvl, _lvlNumber, cost);
                
                case ELvlUpMode.nextPassive:
                    _lvlNumber = Balance.Instance.LevelPerPassivePoint -
                                 (Building.Lvl % Balance.Instance.LevelPerPassivePoint);
                    return GetCostFor(Building.HasCostSteps,Building.Lvl, _lvlNumber, cost);
                
                case ELvlUpMode.max:
                    return GetMaxLvlBuyable();
                
                default:
                    _lvlNumber = 1;
                    return cost;
            }
        }

        public static Resource GetCostFor(bool costSteps, int actualLvl, int lvlUps, Resource actualCost)
        {
            Resource totalCost = new Resource(actualCost);
            Resource nextLvlCost = new Resource(actualCost);
            int lvl = actualLvl;
            
            for (int i = 1; i < lvlUps; i++)
            {
                nextLvlCost = Building.SetNewCost(costSteps, lvl, nextLvlCost);
                totalCost += nextLvlCost;
                lvl++;
            }

            return totalCost;
        }

        private Resource GetMaxLvlBuyable()
        {
            _lvlNumber = 0;
            Resource totalCost = new Resource();
            Resource nextLvlCost = new Resource(Building.Cost);

            do
            {
                _lvlNumber++;
                totalCost += new Resource(nextLvlCost);
                nextLvlCost = Building.SetNewCost(Building.HasCostSteps, Building.Lvl, nextLvlCost);
            } while (Bank.CanPay(totalCost + nextLvlCost));

            return totalCost;
        }

        public void Initialize(BuildingSO building, BuildingTree tree)
        {
            Building = new Building(building);
            tree.Initialize(Building);

            _icon.sprite = building.Icon;
            _nameTxt.text = building.BuildingName;
            UpdateDisplay(Building.ProductionTotal, Building.OnClickTotal);
        
            Building.eUpdate += UpdateDisplay;
            Bank.eOnStockChange += UpdateButtonDisplay;
            Building.eOnPassivePointChange += UpdateTreeButton;
            Clover.Clover.eLuckyLevel += Building.FreeLevelUp;
        }

        private void UpdateTreeButton(Building arg1, int arg2)
        {
            if (Building != arg1) return;
            _passivePoint.text = Building.PassivePoint > 0 ? "" + Building.PassivePoint : "";
        }

        private void UpdateDisplay(Resource productionActual, Resource onClick)
        {
            _lvlTxt.text = "Lvl " + Building.Lvl;
            string production = "";
            if(productionActual.GetTotal() != 0) production += $"Production /s:\n{productionActual.ToString()}";
            if(onClick.GetTotal() != 0) production += $"Production /click:\n{onClick.ToString()}";
            _productionTxt.text = production;
            _treeButton.color = Building.PassivePoint > 0 ? Color.yellow : Color.grey;
            _passivePoint.text = Building.PassivePoint > 0 ? "" + Building.PassivePoint : "";
        }
        
        private void UpdateButtonDisplay(Resource stock)
        {
            Resource cost = GetActualCost();
            _costTxt.text = cost.ToString();
            _nextLvlTxt.text = $"Next +{_lvlNumber} cost";
            _cost.color = Bank.CanPay(cost) ? enable : disable;
        }

        public void LevelUpBtn()
        {
            Resource cost = GetActualCost();
            if (!Bank.CanPay(cost)) return;
            Building.LevelUp(_lvlNumber);
        }

        public void OpenTree()
        {
            eOpenTree?.Invoke(Building.BuildingSO);
        }
    }
}