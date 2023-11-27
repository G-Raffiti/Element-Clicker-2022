using System;
using System.Collections.Generic;
using BigNumbers;
using Singletons;
using UnityEngine;

namespace Trades
{
    public class Bank
    {
        // The Bank resources that are available to purchase anything
        private Resource _stock;
    
        // this is the maximum you can store in the bank (a way to limit the player over power) 
        private Resource _maxStock;
    
        // Singleton
        private static Bank s_instance;
        public static event Action<Resource> eOnStockChange;
        public static event Action<Resource> eOnProduce;
        
        public static Bank Instance
        {
            get
            {
                if (s_instance == null)
                {
                    s_instance = new Bank();
                }

                return s_instance;
            }
        }

        public static Resource Stock => Instance._stock;
        public static Resource MaxStock => Instance._maxStock;

        // Constructor
        private Bank()
        {
            _stock = new Resource();
            _maxStock = new Resource(Balance.Instance.StartMaxBankStock);
        }
    
        // the following method are used to try to Pay or to earn resource
        public static bool Pay(Resource cost)
        {
            if (!CanPay(cost)) return false;
            Instance._stock -= cost;
            eOnStockChange?.Invoke(Instance._stock);
            return true;
        }
        private static bool Pay(EResource resource, bn costValue)
        {
            if (Instance._stock[resource] < costValue) return false;
            Instance._stock[resource] -= costValue;
            eOnStockChange?.Invoke(Instance._stock);
            return true;
        }

        public static void Produce(Resource production)
        {
            foreach (EResource resource in production.Keys)
            {
                Instance._stock[resource] = bn.Min(Instance._stock[resource] + production[resource], Instance._maxStock[resource]);
            }
            eOnProduce?.Invoke(production);
            eOnStockChange?.Invoke(Instance._stock);
        }

        public static bool CanPay(Resource cost)
        {
            return cost <= Instance._stock;
        }

        public static bool IsFull(EResource resource)
        {
            return Instance.StockPercent(resource) >= 1;
        }

        // At least this method is there to Upgrade te Max Resources that the bank can hold
        public static void UpgradeMax(EResource resource, bool isFree)
        {
            if(!isFree) if (!Pay(resource, Instance._maxStock[resource] * Balance.Instance.UpgradeMaxStockCost)) return;
            
            Instance._maxStock[resource] *= Balance.Instance.UpgradeMaxStockFactor;
            Instance._maxStock[resource] += 1;
            
            switch (resource)
            {
                case EResource.Nature:
                    Instance._maxStock[EResource.Air] = Instance._maxStock[EResource.Nature];
                    break;
                case EResource.Water:
                    Instance._maxStock[EResource.Time] = Instance._maxStock[EResource.Water];
                    break;
                case EResource.Fire:
                    Instance._maxStock[EResource.Rage] = Instance._maxStock[EResource.Fire];
                    break;
            }
            
        }

        public float StockPercent(EResource resource)
        {
            if (_maxStock[resource] == 0) return 0;
            return Mathf.Clamp01(_stock[resource] / _maxStock[resource]);
        }

        public static List<EResource> BasicResources { get; } = new List<EResource>()
            { EResource.Gold, EResource.Earth, EResource.Fire, EResource.Nature, EResource.Water, EResource.Tech };
        
        public static List<EResource> SecondResources { get; } = new List<EResource>()
            { EResource.Air, EResource.Time, EResource.Rage };

        public static void LoadData(Resource bankStock, Resource bankMaxStock)
        {
            Instance._maxStock = new Resource(bankMaxStock);
            Instance._stock = new Resource(bankStock);
            eOnStockChange?.Invoke(Instance._stock);
        }
    }
}