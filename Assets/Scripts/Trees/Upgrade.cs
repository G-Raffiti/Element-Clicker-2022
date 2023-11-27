using System;
using System.Collections.Generic;
using Buildings;
using Plugins.LeanTween.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Upgrades;

namespace Trees
{
    public class Upgrade : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _tittle;
        [SerializeField] private TextMeshProUGUI _lvl;
        [SerializeField] private List<Image> _toColor;
        [SerializeField] private Image _icon;
        [SerializeField] private Image _symbol;
        [SerializeField] private CanvasGroup _group;
        
        
        private UpgradeSO _upgrade;
        private List<UpgradeSO> _needed = new List<UpgradeSO>();
        private Building _building;
        private event Action<UpgradeSO> OnUpgradePurchased;

        private bool _initialized = false;

        private int getLvl()
        {
            if (!_building.ActualUpgradesLvl.ContainsKey(_upgrade))
                return 0;
            return _building.ActualUpgradesLvl[_upgrade];
        }

        public void Initialize(UpgradeSO upgrade, Building building, BuildingTreeSO tree)
        {
            _upgrade = upgrade;
            foreach (int index in upgrade.Needed)
            {
                UpgradeSO need = tree.Upgrades.Find(u => u.Number == index);
                if (need != null)
                    _needed.Add(need);
            }
            _building = building;
            OnUpgradePurchased += _building.Upgrade;

            _tittle.text = _upgrade.UpgradeName;
            _lvl.text = "0";
            foreach (Image image in _toColor)
            {
                image.color = _upgrade.Color;
            }
            _symbol.sprite = _upgrade.Symbol;
            if(_symbol.sprite == null)
                _symbol.gameObject.SetActive(false);
            _icon.sprite = _upgrade.Icon;

            _initialized = true;
        }

        public void OnClick()
        {
            if (!IsAccessible()) return;
            OnUpgradePurchased?.Invoke(_upgrade);
        }

        private bool IsAccessible()
        {
            if (!_initialized) return false;
            if (!_building.ActualUpgradesLvl.ContainsKey(_upgrade))
            {
                if (_building.PassivePoint < 1)
                    return false;
            }
            else if (_building.PassivePoint < _building.ActualUpgradesLvl[_upgrade] + 1) return false;
            if (_needed.Count == 0) return true;
            foreach (UpgradeSO upgradeNeeded in _needed)
            {
                if (_building.ActualUpgradesLvl.ContainsKey(upgradeNeeded)) return true;
            }
            return false;
        }
        
        private void Awake()
        {
            if (_initialized) _lvl.text = "" + getLvl();
            Building.eOnPassivePointChange += UpdateLvlTxt;
        }

        private void UpdateLvlTxt(Building arg1, int arg2)
        {
            _group.alpha = IsAccessible() ? 1 : 0.6f;
            _lvl.text = "" + getLvl();
        }

        private void OnDestroy()
        {
            OnUpgradePurchased -= _building.Upgrade;
            Building.eOnPassivePointChange -= UpdateLvlTxt;
        }
    }
}