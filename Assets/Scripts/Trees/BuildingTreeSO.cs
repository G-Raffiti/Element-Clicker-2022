using System;
using System.Collections.Generic;
using UnityEngine;
using Upgrades;

namespace Trees
{
    [CreateAssetMenu(fileName = "Tree_", menuName = "Scriptable Object/Tree")]
    public class BuildingTreeSO : ScriptableObject
    {
        [SerializeField] private List<UpgradeSO> _upgrades;
        public List<UpgradeSO> Upgrades => _upgrades;
    }
}