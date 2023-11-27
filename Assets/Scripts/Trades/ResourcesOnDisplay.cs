using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Trades
{
    public class ResourcesOnDisplay : MonoBehaviour
    {
        [SerializeField] private GameObject ResourcesDisplay;
        [SerializeField] private List<UIStock> _otherResources;
        
        private List<UIStock> _uiStocks = new List<UIStock>();
        private RectTransform _layout;

        private void Start()
        {
            foreach (UIStock child in ResourcesDisplay.GetComponentsInChildren<UIStock>())
            {
                _uiStocks.Add(child);
                child.gameObject.SetActive(false);
            }
            foreach (UIStock child in _otherResources)
            {
                _uiStocks.Add(child);
                child.gameObject.SetActive(false);
            }
            _layout = ResourcesDisplay.GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            Bank.eOnStockChange += UpdateStockDisplay;
        }

        private void UpdateStockDisplay(Resource stock)
        {
            foreach (UIStock ui in _uiStocks)
            {
                if(!ui.gameObject.activeSelf)
                {
                    if (stock[ui.Resource] != 0)
                    {
                        ui.gameObject.SetActive(true);
                        LayoutRebuilder.ForceRebuildLayoutImmediate(_layout);
                        ui.UpdateDisplay(stock[ui.Resource]);
                    }
                    continue;
                }
                
                ui.UpdateDisplay(stock[ui.Resource]);
            }
        }
    }
}