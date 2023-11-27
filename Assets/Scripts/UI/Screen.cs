using System;
using UnityEngine;

namespace UI
{
    [AddComponentMenu("UI/Extensions/Screen")]
    public class Screen : MonoBehaviour
    {
        [SerializeField] private string _screenName;
        public string ScreenName => _screenName;
        public int Index;// { get; private set; }

        private void Start()
        {
            Index = transform.GetSiblingIndex();
        }
    }
}