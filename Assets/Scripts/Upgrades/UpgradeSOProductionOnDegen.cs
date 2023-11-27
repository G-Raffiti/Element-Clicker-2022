using System;
using BigNumbers;
using Trades;
using UnityEngine;

namespace Upgrades
{
    [CreateAssetMenu(fileName = "BUILDING_ID_Upgrade_OnDegen_NAME", menuName = "Scriptable Object/Upgrades/OnDegen")]
    public class UpgradeSOProductionOnDegen : UpgradeSOSpecial
    {
        [SerializeField] private float _ratio;
        [SerializeField] private EResource _relatedTo;
        private bn _degenValue;
        
        private void OnEnable()
        {
            SpecialUpgradeFactors.eOnDegenChangeValue += SendRatio;
        }

        private void SendRatio(EResource resource, bn value)
        {
            if (_relatedTo != resource) return;
            _degenValue = value;

            SendRatio();
        }

        public override Production GetRatio(int lvl)
        {
            Production prod = new Production();
            foreach (EResource resource in Enum.GetValues(typeof(EResource)))
            {
                prod[resource] = new bn2(0, _degenValue * (_ratio * lvl), 0);
            }

            return prod;
        }
    }
}