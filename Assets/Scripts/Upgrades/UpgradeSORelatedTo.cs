using System;
using BigNumbers;
using Trades;
using UnityEngine;

namespace Upgrades
{
    [CreateAssetMenu(fileName = "BUILDING_ID_Upgrade_Related_NAME", menuName = "Scriptable Object/Upgrades/RelatedTo")]
    public class UpgradeSORelatedTo : UpgradeSOSpecial
    {
        [SerializeField] private Production.SProductions _ratio = new Production.SProductions();
        [SerializeField] private EResource _relatedTo;
        private Production ratio;

        protected virtual void OnEnable()
        {
            ratio = new Production(_ratio);
            SpecialUpgradeFactors.eOnBankValueChange += SendRatio;
        }

        protected virtual void SendRatio(EResource arg1)
        {
            if (arg1 != _relatedTo) return;
            
            SendRatio();
        }

        public override Production GetRatio(int lvl)
        {
            Production prod = new Production();
            foreach (EResource resource in Enum.GetValues(typeof(EResource)))
            {
                prod[resource] = new bn2(
                    Bank.Stock[_relatedTo] * (ratio[resource].Added * lvl), 
                    bn.Max(1,Bank.Stock[_relatedTo] * (ratio[resource].Multi * lvl)),
                    Bank.Stock[_relatedTo] * (ratio[resource].Increased * lvl));
            }
            return prod;
        }
    }
}