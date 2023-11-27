using System;
using System.Collections;
using System.Collections.Generic;
using _Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ScrollUI : MonoBehaviour
    {
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private RectTransform contentPanel;
        [SerializeField] private float _objectWidth;
        [SerializeField] private float _offset = 12.5f;
        public bool _isMoving;
        private bool _isOnTheWay;
        private Dictionary<string, int> _screens = new Dictionary<string, int>();

        public int _currentIndex;
        private int _total => contentPanel.childCount;

        public Vector3 _destination;

        private void Start()
        {
            foreach (RectTransform child in contentPanel)
            {
                Screen screen = child.GetComponent<Screen>();
                _screens.Set(screen.ScreenName, screen.Index);
            }
            ButtonGoTo.eGoToScreen += GoTo;
            ButtonScroll.eGoToNext += GoNext;
            _currentIndex = GetCurrentIndex();
            _destination = contentPanel.localPosition;
        }

        private void Update()
        {
            _currentIndex = GetCurrentIndex();
            if (contentPanel.localPosition == _destination) return;
            if (Input.anyKey) return;
            if (_isOnTheWay) return;
            
            SnapToNearest();
        }

        private void FixedUpdate()
        {
            if (_isMoving)
            {
                Vector3 direction = (_destination - contentPanel.localPosition);
                if (Mathf.Abs(contentPanel.localPosition.x - _destination.x) <= 6f)
                {
                    _destination.x = - _currentIndex * _objectWidth + _offset;
                    contentPanel.localPosition = _destination;
                    _isMoving = false;
                    _isOnTheWay = false;
                    return;
                }
                contentPanel.Translate(direction * ( 0.05f * Time.fixedDeltaTime));
            }
        }
        
        private int GetCurrentIndex()
        {
            float position = contentPanel.localPosition.x;
            int positionRelative = Mathf.RoundToInt(position / _objectWidth);
            int index = - positionRelative % _total;
            if (index < 0)
                index += _total;
            return index;
        }
        
        private void SnapToNearest()
        {
            if(_currentIndex == _total-1 && contentPanel.localPosition.x > 0)
                _destination.x = _objectWidth + _offset; 
            else if(_currentIndex == 0 && contentPanel.localPosition.x < - _objectWidth)
                _destination.x = - _total * _objectWidth + _offset;
            else
                _destination.x = - _currentIndex * _objectWidth + _offset;
            _isMoving = true;
        }

        private void GoToIndex(int target)
        {
            _isOnTheWay = true;
            Vector3 currentPos = contentPanel.localPosition;
            Vector3 newPos = currentPos;

            int steps;
            if (Mathf.Abs(target - _currentIndex) <= _total / 2f)
            {
                steps = -(target - _currentIndex);
            }
            else
            {
                if (target > _currentIndex)
                {
                    steps = -(target - _currentIndex - _total);
                }
                else
                {
                    steps = -(_total - (_currentIndex - target));
                }
            }

            newPos.x += steps * _objectWidth;
            _destination = newPos;
            _isMoving = true;
        }

        private void GoTo(string screenName)
        {
            GoToIndex(_screens[screenName]);
        }
        
        private void GoNext(bool obj)
        {
            if (obj) GoToIndex(_currentIndex - 1);
            else GoToIndex(_currentIndex + 1);
        }
    }
}