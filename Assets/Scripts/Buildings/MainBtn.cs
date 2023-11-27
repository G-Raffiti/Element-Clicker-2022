using System;
using System.Collections.Generic;
using System.Linq;
using _Extensions;
using BigNumbers;
using Singletons;
using Trades;
using UnityEngine;
using UnityEngine.UI;
using Upgrades;

namespace Buildings
{
    public class MainBtn : MonoBehaviour
    {
        [SerializeField] private Button button;
        private bn _critMulti = 2;
        private Resource _onClick = new Resource();

        [SerializeField] private Resource.SResource _baseChance;
        private Resource _chance = new Resource();
        
        public static event Action eOnClick;
        public static event Action<EResource, string> eOnClicked;
        private Dictionary<BuildingSO, Resource> _productionOnClick = new Dictionary<BuildingSO, Resource>();

        private void Awake()
        {
            Building.eOnClickProductionChanged += UpdateProductionOnClick;
            UpgradeSOChanceOnClick.eChanceValue += UpdateChance;
            _chance = new Resource(_baseChance);
            button.onClick.AddListener(OnClick);
        }

        private void UpdateChance(EResource arg1, int arg2)
        {
            _chance[arg1] += arg2;
        }


        private void UpdateProductionOnClick(Building arg1, Resource arg2)
        {
            _productionOnClick.Set(arg1.BuildingSO, arg2);
            _onClick = new Resource();
            
            foreach (BuildingSO building in _productionOnClick.Keys)
            {
                _onClick += _productionOnClick[building];
            }
        }

        public void OnClick()
        {
            Resource earned = new Resource(_onClick);
            earned[EResource.Gold] *= 1 + (_critMulti * Random((int)_chance[EResource.Gold].Value));
            ElementChance(earned);
            ConsumeTime(earned);
            earned[EResource.Click] += 1;
            Bank.Produce(earned);
            eOnClick?.Invoke();
            foreach (EResource resource in earned.Keys.Where(r => earned[r] != 0))
            {
                eOnClicked?.Invoke(resource, $"+{earned[resource]}");
            }
            if(Bank.IsFull(EResource.Click))
                Bank.UpgradeMax(EResource.Click, true);
        }

        private void ConsumeTime(Resource earned)
        {
            if (Bank.Stock[EResource.Time] <= 1) return;
            earned[EResource.Time] = Balance.Instance.TimeOnClick * Bank.Stock[EResource.Time];
        }

        private void ElementChance(Resource earned)
        {
            earned[EResource.Earth] *= Random((int)_chance[EResource.Earth]);
            earned[EResource.Fire] *= Random((int)_chance[EResource.Fire]);
            earned[EResource.Nature] *= Random((int)_chance[EResource.Nature]);
            earned[EResource.Water] *= Random((int)_chance[EResource.Water]);
            earned[EResource.Tech] *= Random((int)_chance[EResource.Tech]);
        }

        private float Random(int chance)
        {
            if (chance < 100)
            {
                int test = UnityEngine.Random.Range(0, 101);

                return chance >= test ? 1 : 0;
            }

            return (int) (chance / 100f);
        }

    }
}