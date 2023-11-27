using System;
using System.Collections.Generic;
using Buildings;
using Trees;
using UnityEditor;
using UnityEngine;
using Upgrades;

namespace CSV.Editor
{
    public static class SOGenerator
    {
        [MenuItem("Data to Object/Generate Upgrades from CSV")]
        public static void GenerateUpgrades()
        {
            if (Selection.activeObject == null) return;
            string path = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(),
                AssetDatabase.GetAssetPath(Selection.activeObject));

            if (!IsCSVFile(path))
            {
                Debug.LogError("you need to select a valid CSV File");
                return;
            }

            List<Dictionary<string, object>> rawData = CSVReader.Read(path);

            if (rawData.Count <= 0)
            {
                Debug.LogError("you need to select a valid CSV File");
                return;
            }

            bool confirmed = EditorUtility.DisplayDialog("Creation of the Upgrades as Scriptable Objects",
                $"Are you sure you want to create {rawData.Count} Upgrades ?", "Yes", "No");
            if (confirmed)
            {
                PerformGenerationUpgrade(rawData);
            }
        }

        private static void PerformGenerationUpgrade(List<Dictionary<string, object>> csvData)
        {
            for (int i = 0; i < csvData.Count; i++)
            {
                Dictionary<string, object> _potentialUpgrade = csvData[i];
                CreateScriptableObjectUpgrade(_potentialUpgrade);
            }
        }

        private static UpgradeSO CreateScriptableObjectUpgrade(Dictionary<string, object> csvUpgrade)
        {
            
            UpgradeSO newUpgrade = null;
            rawUpgrade rawUpgrade = new rawUpgrade(csvUpgrade);
            switch (rawUpgrade.Type)
            {
                case EUpgradeType.Unlock: 
                    newUpgrade = ScriptableObject.CreateInstance<UpgradeSOUnlock>();
                    break;
                case EUpgradeType.Production: 
                    newUpgrade = ScriptableObject.CreateInstance<UpgradeSOProduction>();
                    break;
                case EUpgradeType.Related:
                    newUpgrade = ScriptableObject.CreateInstance<UpgradeSORelated>();
                    break;
                case EUpgradeType.TownLvl:
                    ScriptableObject.CreateInstance<UpgradeSOTownLvl>();
                    break;
                default:
                    newUpgrade = ScriptableObject.CreateInstance<UpgradeSOProduction>();
                    break;
            }
            
            newUpgrade.SetDATA(rawUpgrade);

            string path = $"Assets/Resources/ScriptableObjects/Trees/{rawUpgrade.Building}_{rawUpgrade.Number}" +
                          $"_Upgrade_{rawUpgrade.Type}_{rawUpgrade.UpgradeName}.asset";

            AssetDatabase.CreateAsset(newUpgrade, path); 
            AssetDatabase.SaveAssets();

            return Resources.Load<UpgradeSO>(path);
        }
        
        private static bool IsCSVFile(string path)
        {
            return path.ToLower().EndsWith(".csv");
        }
    }
}