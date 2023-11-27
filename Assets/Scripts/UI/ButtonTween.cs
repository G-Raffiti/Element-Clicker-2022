using System;
using System.Collections;
using Plugins.LeanTween.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class ButtonTween : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private bool _isMultilayer;
        
        private void Start()
        {
            if(!_isMultilayer) return;
            foreach (Image image in GetComponentsInChildren<Image>())
            {
                image.raycastTarget = false;
            }
            foreach (TextMeshProUGUI text in GetComponentsInChildren<TextMeshProUGUI>())
            {
                text.raycastTarget = false;
            }

            if (GetComponent<Image>() != null)
                GetComponent<Image>().raycastTarget = true;
            else
            {
                if (GetComponent<TextMeshProUGUI>() != null)
                    GetComponent<TextMeshProUGUI>().raycastTarget = true;
                else 
                    gameObject.AddComponent<Image>().color = Color.clear;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            LeanTween.scale(gameObject, Vector3.one * 1.1f, 0.1f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            LeanTween.scale(gameObject, Vector3.one, 0.1f);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            StopCoroutine(OnClick());
            if(!gameObject.activeSelf) return;
            StartCoroutine(OnClick());
        }

        private IEnumerator OnClick()
        {
            LeanTween.scale(gameObject, Vector3.one * 1.1f, 0f);
            LeanTween.scale(gameObject, Vector3.one * 1.15f, 0.1f);
            yield return new WaitForSeconds(0.1f);
            LeanTween.scale(gameObject, Vector3.one * 1.1f, 0.1f);
        }

        private void OnEnable()
        {
            LeanTween.scale(gameObject, Vector3.one, 0);
        }
    }
}
