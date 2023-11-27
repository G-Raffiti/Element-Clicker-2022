using System;
using _ScreenResolution;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.EventSystems;

namespace UI
{
    public class ButtonScroll : MonoBehaviour, IPointerClickHandler
    {
        private enum ENext
        {
            Previous, Next,
        }
        [SerializeField] private ENext _direction;
        public static event Action<bool> eGoToNext;
        private Camera _camera;
        private int _screenOffsetX;

        private void Start()
        {
            _camera = Camera.main;
            CameraResolution.eCameraOffsetX += UpdateOffset;
        }

        private void UpdateOffset(int value)
        {
            _screenOffsetX = value;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            float pos = _camera.WorldToScreenPoint(transform.position).x;
            
            switch (_direction)
            {
                case ENext.Previous:
                    if(pos < 50 + _screenOffsetX)
                        eGoToNext?.Invoke(true);
                    break;
                case ENext.Next:
                    if (pos > UnityEngine.Screen.width - 50 - _screenOffsetX)
                        eGoToNext?.Invoke(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}