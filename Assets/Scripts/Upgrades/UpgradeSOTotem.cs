using System;
using BigNumbers;
using Buildings;
using Managers;
using Trades;
using UnityEngine;

namespace Upgrades
{
    [CreateAssetMenu(fileName = "BUILDING_ID_Upgrade_Totem_NAME", menuName = "Scriptable Object/Upgrades/Totem")]
    public class UpgradeSOTotem : UpgradeSORelated
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            ProductionManager.eOnTotemOff += SendRatio;
            Totem.eOnSwitch += SendRatio;
        }

        private void SendRatio(bool obj)
        {
            base.SendRatio(EResource.Air);
        }
        
        public override Production GetRatio(int lvl)
        {
            Production prod = new Production();
            if (!Totem.IsActive) return prod;
            
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