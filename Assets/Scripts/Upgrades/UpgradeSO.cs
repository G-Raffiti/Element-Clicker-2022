using Buildings;
using CSV;
using Trades;
using UnityEngine;

namespace Upgrades
{
    public abstract class UpgradeSO : ScriptableObject
    {
        [SerializeField] private int _number;
        public int Number => _number;
        [SerializeField] private int[] _needed;
        public int[] Needed => _needed;
        
        [SerializeField] private Sprite _icon;
        public Sprite Icon => _icon;
    
    
        [SerializeField] private string _upgradeName;
        public string UpgradeName => _upgradeName;


        [SerializeField] private int _maxLvl;
        public int MaxLvl => _maxLvl;
        
        
        [SerializeField] private Color color;
        public Color Color => color;
        
        
        [SerializeField] private Sprite symbol;
        public Sprite Symbol => symbol;

        public virtual void OnUpgrade(Building building, int upgradeLvl)
        {
            _building = building;
        }
        protected Building _building;
        
        [Multiline(5)]
        [SerializeField] private string Description;
        [SerializeField] private Resource.SResource _addedCost;
        public Resource AddedCost => new Resource(_addedCost);

        public virtual void SetDATA(rawUpgrade newData)
        {
            _number = newData.Number;
            _icon = newData.Icon;
            _upgradeName = newData.UpgradeName;
            _maxLvl = newData.MaxLvl;
        }
    }
}