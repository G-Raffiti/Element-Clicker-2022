using System;
using System.Collections.Generic;
using Buildings;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Upgrades;

namespace Trees
{
    public class BuildingTree : MonoBehaviour
    {
        private BuildingTreeSO _treeSO;

        [SerializeField] private List<Transform> _upgradeHolders = new List<Transform>();
        [SerializeField] private Upgrade _upgrade_PF;
        [SerializeField] private TextMeshProUGUI _passivePoint;

        public void Initialize(Building building)
        {
            _treeSO = building.BuildingSO.Tree;
            Dictionary<int, Transform> holders = new Dictionary<int, Transform>();
            for (int i = 0; i < _upgradeHolders.Count; i++)
            {
                holders.Add(i, _upgradeHolders[i]);
                _upgradeHolders[i].GetComponent<Image>().enabled = false;
                _upgradeHolders[i].GetComponentInChildren<TextMeshProUGUI>().enabled = false;
                
            }
        
            foreach (UpgradeSO upgrade in _treeSO.Upgrades)
            {
                GameObject prefab = Instantiate(_upgrade_PF.gameObject, holders[upgrade.Number]);
                prefab.GetComponent<Upgrade>().Initialize(upgrade, building, _treeSO);
            }
            
            Building.eOnPassivePointChange += UpdatePassivePoints;
            _passivePoint.text = "Passive Points: 0";
        }

        void UpdatePassivePoints(Building building, int points)
        {
            _passivePoint.text = "Passive Points: " + building.PassivePoint;
        }
    }
}