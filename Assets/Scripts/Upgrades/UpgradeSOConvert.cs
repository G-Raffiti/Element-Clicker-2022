using System;
using BigNumbers;
using Buildings;
using Trades;
using UnityEngine;

namespace Upgrades
{
    [CreateAssetMenu(fileName = "BUILDING_ID_Upgrade_Convert_NAME", menuName = "Scriptable Object/Upgrades/Convert")]
    public class UpgradeSOConvert : UpgradeSO
    {
        [SerializeField] private EResource _from;
        [SerializeField] private EResource _to;
        [SerializeField] private float _ratio;

        public override void OnUpgrade(Building building, int upgradeLvl)
        {
            base.OnUpgrade(building, upgradeLvl);
            building.RegisterConvertUpgrade(this);
        }

        public Resource Convert(Resource buildingProduction, int lvl)
        {
            Resource newProduction = new Resource();

            foreach (EResource resource in Enum.GetValues(typeof(EResource)))
            {
                if (resource != _from)
                    newProduction[resource] = new bn(buildingProduction[resource]);
            }

            float ratioTotal = Mathf.Min(1, lvl * _ratio);
            newProduction[_to] += new bn(buildingProduction[_from] * ratioTotal);
            newProduction[_from] -= new bn(buildingProduction[_from] * ratioTotal);

            return newProduction;
        }
    }
}