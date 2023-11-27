using System;
using BigNumbers;
using Trades;
using UnityEngine;

namespace Upgrades
{
    [CreateAssetMenu(fileName = "BUILDING_ID_Upgrade_Related_NAME", menuName = "Scriptable Object/Upgrades/Related")]
    public class UpgradeSORelated : UpgradeSOSpecial
    {
        [SerializeField] protected bn2 _ratio;
        [SerializeField] protected EResource _relatedTo;

        protected virtual void OnEnable()
        {
            SpecialUpgradeFactors.eOnBankValueChange += SendRatio;
        }

        protected virtual void SendRatio(EResource arg1)
        {
            if (_relatedTo != arg1) return;
            
            SendRatio();
        }

        public override Production GetRatio(int lvl)
        {
            Production prod = new Production();
            foreach (EResource resource in Enum.GetValues(typeof(EResource)))
            {
                prod[resource] = new bn2(
                    Bank.Stock[_relatedTo] * (_ratio.Added * lvl), 
                    bn.Max(1,Bank.Stock[_relatedTo] * (_ratio.Multi * lvl)),
                    Bank.Stock[_relatedTo] * (_ratio.Increased * lvl));
            }
            return prod;
        } 
    }
}