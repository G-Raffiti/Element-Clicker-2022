using System;
using System.Collections;
using System.Collections.Generic;
using _Extensions;
using Buildings;
using Singletons;
using Trades;
using UI;
using UnityEngine;
using Upgrades;

namespace Managers
{
    public class ProductionManager : MonoBehaviour
    {
        private Dictionary<Building, Resource> _productors = new Dictionary<Building, Resource>();
        private float _cloverBoost = 1;

        private Resource _productionPerSecond = new Resource();
        private Resource _productionOnTick = new Resource();

        private Resource _degen = new Resource();
        public static event Action eOnTotemOff;
        public static event Action<Resource> eOnDegenUpdated; 

        private void Start()
        {
            StartCoroutine(Produce());
            Building.eOnProductionChanged += UpdateProduction;
            SpecialUpgradeFactors.eOnBankValueChange += UpdateDegen;
            SettingsUI.eOnTickRateChange += UpdateProduction;
            Totem.eOnSwitch += UpdateDegen;
            Clover.Clover.eLuckyProductionMultiForTime += StartBoost;
        }

        private IEnumerator Produce()
        {
            yield return new WaitForSeconds(Settings.Instance.Tick);
            Bank.Produce(_productionOnTick - _degen);
            yield return Produce();
        }

        private void UpdateProduction(Building building, Resource resource)
        {
            _productors.Set(building, resource);
            UpdateProduction();
        }

        private void UpdateProduction()
        {
            Resource result = new Resource();
            foreach (Building build in _productors.Keys)
            {
                result += _productors[build];
            }

            result *= _cloverBoost;
            
            _productionPerSecond = new Resource(result);
            _productionOnTick = _productionPerSecond * Settings.Instance.Tick;
        }
        
        private void StartBoost(float multiplier, float time)
        {
            _cloverBoost *= multiplier;
            StartCoroutine(Boost(multiplier, time));
            UpdateProduction();
        }

        private IEnumerator Boost(float multiplier, float time)
        {
            yield return new WaitForSeconds(time);
            _cloverBoost *= 1 / multiplier;
            UpdateProduction();
        }

        private void UpdateDegen(bool obj)
        {
            UpdateDegen(EResource.Air);
        }

        private void UpdateDegen(EResource resource)
        {
            switch(resource)
            {
                case EResource.Air:
                    PsyDegen();
                    break;
                case EResource.Rage:
                    RageDegen();
                    break;
            }
            
            eOnDegenUpdated?.Invoke(_degen);
        }

        private void RageDegen()
        {
            if(Bank.Stock[EResource.Rage] == 0) return;
            if (Bank.Stock[EResource.Rage] <= 1)
            {
                _degen[EResource.Rage] = 0;
                return;
            }
            
            _degen[EResource.Rage] = Bank.Stock[EResource.Rage] * 0.1f;
            if (Bank.Stock[EResource.Rage] >= RunStats.Instance.Produced[EResource.Click])
            {
                _degen[EResource.Rage] += Bank.Stock[EResource.Rage] * 0.4f;
            }

            _degen[EResource.Rage] *= Settings.Instance.Tick;
        }

        private void PsyDegen()
        {
            if(Bank.Stock[EResource.Air] == 0) return;
            _degen[EResource.Air] = Bank.Stock[EResource.Air] * 0.01;
            
            if(TotalStats.Instance.Produced[EResource.Air] > 100)
                if (Bank.Stock[EResource.Air] > TotalStats.Instance.Produced[EResource.Air] * 0.01)
                    _degen[EResource.Air] += Bank.Stock[EResource.Air] * 0.1;
            if (Totem.IsActive)
            {
                _degen[EResource.Air] += Bank.Stock[EResource.Air] * 0.15;
                if (Bank.Stock[EResource.Air] <= 1)
                {
                    _degen[EResource.Air] = Bank.Stock[EResource.Air];
                    eOnTotemOff?.Invoke();
                    UpdateProduction();
                }
            }
            
            _degen[EResource.Air] *= Settings.Instance.Tick;
        }
    }
}