using System;
using Buildings;
using Trades;

namespace Upgrades
{
    public abstract class UpgradeSOSpecial : UpgradeSO
    {
        public abstract Production GetRatio(int lvl);
        private event Action<UpgradeSOSpecial, Production> eOnRatioChanged;

        public override void OnUpgrade(Building building, int upgradeLvl)
        {
            base.OnUpgrade(building, upgradeLvl);
            if(upgradeLvl == 1)
                eOnRatioChanged += building.UpdateProductionVariation;
            eOnRatioChanged?.Invoke(this, GetRatio(upgradeLvl));
        }

        protected void SendRatio()
        {
            eOnRatioChanged?.Invoke(this, GetRatio(_building.ActualUpgradesLvl[this]));
        }
    }
}