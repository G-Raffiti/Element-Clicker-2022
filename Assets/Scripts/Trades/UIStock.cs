using _Extensions;
using BigNumbers;
using Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Trades
{
    public class UIStock : MonoBehaviour
    {
        [SerializeField] private EResource resource;
        public EResource Resource => resource;
        [SerializeField] private TextMeshProUGUI _valueTxt;
        [SerializeField] private Gradient _color;
        [SerializeField] private Image _fillBar;
        [SerializeField] private Button _button;
        [SerializeField] private Image _buttonImage;

        private bn _lastValue = 1;

        private void Start()
        {
            _button.onClick.AddListener(() => UpgradeMax(resource));
            UpdateDisplay(0);
        }

        public void UpdateDisplay(bn value)
        {
            //if (_lastValue == value) return;

            _valueTxt.text = $"{value.ToString(Settings.Instance.Format)} {resource.String()}";
            
            float fillAmount = Bank.Instance.StockPercent(resource);
            _fillBar.color = _color.Evaluate(fillAmount);
            _fillBar.fillAmount = fillAmount;

            _buttonImage.color = (fillAmount >= Balance.Instance.UpgradeMaxStockCost) ? Color.yellow : Color.grey;
            //_lastValue = value;
        }

        private void UpgradeMax(EResource resource)
        {
            Bank.UpgradeMax(resource, false);
        }
    }
}