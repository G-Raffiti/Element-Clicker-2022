using System;
using System.Collections.Generic;
using BigNumbers;
using Buildings;
using Trades;
using UnityEngine;

namespace CSV
{
    public enum EUpgradeType
    {
        Unlock, Production, Related, TownLvl
    }
    public struct rawUpgrade
    {
        public string Building;
        public EUpgradeType Type;
        public int Number;
        public Sprite Icon;
        public string UpgradeName;
        public int MaxLvl;

        public Production Production;
        public BuildingSO BuildingSO;
        
        public rawUpgrade(Dictionary<string, object> csv)
        {
            Building = csv["Building"].ToString();
            UpgradeName = csv["Name"].ToString();
            Enum.TryParse(csv["Type"].ToString(), out Type);
            int.TryParse(csv["Id"].ToString(), out Number);
            if (csv["Icon"].ToString() == string.Empty)
                Icon = null;
            else
                Icon = UnityEngine.Resources.Load<Sprite>(
                $"Icons/{csv["Icon"]}");
            int.TryParse(csv["MaxLvl"].ToString(), out MaxLvl);

            BuildingSO = null;
            Production = new Production();

            switch (Type)
            {
                case EUpgradeType.Unlock :
                        BuildingSO = UnityEngine.Resources.Load<BuildingSO>(
                            $"ScriptableObject/BuildingSO/Building_{csv["Building"]}");
                    break;
                case EUpgradeType.Production :
                    foreach (EResource resource in Enum.GetValues(typeof(EResource)))
                    {
                        float.TryParse(csv[$"{resource}Add"].ToString(), out float added);
                        float.TryParse(csv[$"{resource}Mult"].ToString(), out float multi);
                        float.TryParse(csv[$"{resource}Incr"].ToString(), out float increased);
                        Production[resource] = new bn2(added, multi, increased);
                    }
                    break;
            }
        }
    }
}