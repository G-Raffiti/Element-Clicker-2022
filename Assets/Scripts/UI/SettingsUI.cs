using System;
using BigNumbers;
using Singletons;
using TMPro;
using UnityEngine;

namespace UI
{
    public class SettingsUI : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown _format;
        [SerializeField] private TMP_Dropdown _buyMod;
        [SerializeField] private TMP_Dropdown _inputField;
        public static event Action eOnTickRateChange;

        private void Start()
        {
            _format.onValueChanged.AddListener(FormatChange);
            _buyMod.onValueChanged.AddListener(BuyModChange);
            _inputField.onValueChanged.AddListener(TickSpeed);
            Settings.Instance.eOnLoaded += UpdateDisplay;
        }

        private void UpdateDisplay()
        {
            _format.value = (int)Settings.Instance.Format;
            _format.RefreshShownValue();
            _buyMod.value = (int)Settings.Instance.Format;
            _buyMod.RefreshShownValue();
            switch (Settings.Instance.Tick)
            {
                case 0.1f : _inputField.value = 0;
                    break;
                case 0.2f : _inputField.value = 1;
                    break;
                case 0.5f : _inputField.value = 2;
                    break;
                case 1f : _inputField.value = 3;
                    break;
            }
            _inputField.RefreshShownValue();
        }

        private void TickSpeed(int value)
        {
            float tickRate = 0.2f;
            switch (value)
            {
                case 0 : tickRate = 0.1f;
                    break;
                case 1 : tickRate = 0.2f;
                    break;
                case 2 : tickRate = 0.5f;
                    break;
                case 3 : tickRate = 1f;
                    break;
            }
            Settings.Instance.Tick = tickRate;
            eOnTickRateChange?.Invoke();
        }

        private void BuyModChange(int value)
        {
            Settings.Instance.LvlUpMode = (ELvlUpMode) value;
        }

        void FormatChange(int value)
        {
            Settings.Instance.Format = (EBigNumberFormat) value;
        }

    }
}