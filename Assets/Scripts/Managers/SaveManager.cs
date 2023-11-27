using System;
using _SaveSystem;
using _SaveSystem.Data;
using _SaveSystem.SerializableClasses;
using Singletons;
using Trades;
using UnityEngine;

namespace Managers
{
    public class SaveManager : MonoBehaviour, IDataPersistence
    {
        public double OfflineSeconds { get; private set; }
        public static event Action eOnGameLoaded;
        public void SaveData(GameData data)
        {
            //Bank
            data.BankStock = new ResourceSerializable(Bank.Stock);
            foreach (EResource resource in Bank.SecondResources)
            {
                data.BankStock[resource] = 0;
            }
            data.BankMaxStock = new ResourceSerializable(Bank.MaxStock);
            
            //Time
            data.SaveTime = DateTime.Now.ToBinary();
            
            //RunStats
            data.Run = new StatSerializable(RunStats.Instance);
            data.RunTime = RunStats.Instance.StartRun.ToBinary();
            
            //TotalStats
            data.Total = new StatSerializable(TotalStats.Instance);
            
            //Settings
            data.Settings = new SettingsSerializable(Settings.Instance);
        }

        public void LoadData(GameData data)
        {
            //Bank
            Resource bankMaxStock = data.BankMaxStock.GetResource();
            Resource bankStock = data.BankStock.GetResource();
            Bank.LoadData(bankStock, bankMaxStock);
            
            //Time
            DateTime lastSave = DateTime.FromBinary(data.SaveTime);
            TimeSpan offlineTime = DateTime.Now.Subtract(lastSave);
            OfflineSeconds = offlineTime.TotalSeconds;
            
            //RunStats
            RunStats.Instance.LoadData(data);
            
            //TotalStats
            TotalStats.Instance.LoadData(data);
            
            //Settings
            Settings.Instance.LoadData(data.Settings);
            
            eOnGameLoaded?.Invoke();
        }
    }
}