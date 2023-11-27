using System;
using _Extensions;
using _SaveSystem.SerializableClasses;
using BigNumbers;
using UnityEngine;

namespace Singletons
{
    public enum ELvlUpMode { one, round, nextPassive, max}
    
    [CreateAssetMenu(fileName = "Settings", menuName = "Scriptable Object/Singleton/Settings")]
    public class Settings : ScriptableObjectSingleton<Settings>
    {
        public EBigNumberFormat Format = EBigNumberFormat.Basic;
        public ELvlUpMode LvlUpMode = ELvlUpMode.one;
        public float Tick = 0.2f;
        [SerializeField] public string BuildVersion;
        public event Action eOnLoaded;

        public void LoadData(SettingsSerializable dataSettings)
        {
            Format = dataSettings.Format;
            LvlUpMode = dataSettings.LvlUpMode;
            Tick = dataSettings.Tick;
            eOnLoaded?.Invoke();
        }

        public void SetBuildVersion(string buildVersion)
        {
            BuildVersion = buildVersion;
        }
    }
}
