using System;
using System.Collections.Generic;
using _Extensions;
using TMPro;
using Trades;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI
{
    public class Coin : MonoBehaviour
    {
        [SerializeField] private List<Sprite> _sprites;
        [SerializeField] private List<Sprite> _resources;
        [SerializeField] private TextMeshProUGUI _value;
        [SerializeField] private Image _image;
        [SerializeField] private Animator _anim;
        [SerializeField] private int _max;
        
        private void Start()
        {
            _anim.SetTrigger("" + Random.Range(0, _max));
            Destroy(gameObject, 1f);
        }

        public void SetValue(EResource eResource, string value)
        {
            if(eResource == EResource.Gold)
                _image.sprite = _sprites.GetRandom();
            /*else
            {
                _image.sprite = _resources[(int)eResource];
            }*/
            _value.text = value;
        }
    }
}