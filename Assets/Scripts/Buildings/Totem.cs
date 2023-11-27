using System;
using Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Buildings
{
    public class Totem : MonoBehaviour, IPointerClickHandler
    {
        public static bool IsActive { get; private set; }
        public static event Action<bool> eOnSwitch;
        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
            _image.color = ImageColor;
        }

        private void OnEnable()
        {
            ProductionManager.eOnTotemOff += SwitchTotemOnOff;
        }

        private void SwitchTotemOnOff()
        {
            IsActive = !IsActive;
            eOnSwitch?.Invoke(IsActive);
            _image.color = ImageColor;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            SwitchTotemOnOff();
        }

        private Color ImageColor => IsActive ? Color.green : Color.red;
    }
}