using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    [RequireComponent(typeof(ButtonTween))]
    public class ButtonGoTo : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private string _screenName;
        public static event Action<string> eGoToScreen;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            eGoToScreen?.Invoke(_screenName);
        }
    }
}