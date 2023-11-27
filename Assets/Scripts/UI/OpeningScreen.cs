using System;
using System.Collections;
using System.Collections.Generic;
using _Extensions;
using _SaveSystem;
using Managers;
using Singletons;
using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;

namespace UI
{
    public class OpeningScreen : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _blackScreen;
        [SerializeField] private Image _illu;
        [SerializeField] private Image _progressBar;
        [SerializeField] private float _chargingValue = 0.5f;
        [SerializeField] private List<Sprite> backgrounds;
        [SerializeField] private TextMeshProUGUI buildVersion;
        private float _actualValue = 0;
        public static event Action eGameLoaded;

        private void OnEnable()
        {
            BuildingManager.eOnGameLoaded += UpdateProgressBar;
            SaveManager.eOnGameLoaded += UpdateProgressBar;
        }

        private void Awake()
        {
            _blackScreen.gameObject.SetActive(true);
            _illu.sprite = backgrounds.GetRandom();
            buildVersion.text = "Version " + Settings.Instance.BuildVersion;
        }

        private void UpdateProgressBar()
        {
            _actualValue = Mathf.Clamp01(_actualValue + _chargingValue);
            StartCoroutine(ChangeProgressBar());
        }

        private IEnumerator ChangeProgressBar()
        {
            _progressBar.fillAmount += 0.03f;
            yield return new WaitForFixedUpdate();
            if (Math.Abs(_progressBar.fillAmount - 1) < 0.02f)
            {
                yield return FadeOff();
                yield break;
            }
            if (_progressBar.fillAmount < _actualValue)
                yield return ChangeProgressBar();
        }

        private IEnumerator FadeOff()
        {
            _blackScreen.alpha -= 0.1f;
            yield return new WaitForSecondsRealtime(0.1f);
            if(_blackScreen.alpha < 0.1f)
            {
                _blackScreen.gameObject.SetActive(false);
                eGameLoaded?.Invoke();
                yield break;
            }
            yield return FadeOff();
        }

    }
}