using System;
using System.Collections.Generic;
using _Extensions;
using BigNumbers;
using UnityEngine;

namespace Trades
{
    public class Production : Dictionary<EResource, bn2>
    {
        public Production()
        {
            foreach (EResource resource in Enum.GetValues(typeof(EResource)))
            {
                Add(resource, new bn2());
            }
        }

        public Production(SProductions genericProduction)
        {
            foreach (EResource resource in Enum.GetValues(typeof(EResource)))
            {
                Add(resource, new bn2());
            }
            
            foreach (EResource resource in genericProduction.Keys)
            {
                this[resource] = new bn2(genericProduction[resource]);
            }
        }

        private Production(Production production)
        {
            foreach (EResource resource in Enum.GetValues(typeof(EResource)))
            {
                Add(resource, new bn2());
            }
            
            foreach (EResource resource in production.Keys)
            {
                this[resource] = new bn2(production[resource]);
            }
        }

        public Production(GenericDictionary<EResource, bn2> genericProduction)
        {
            foreach (EResource resource in Enum.GetValues(typeof(EResource)))
            {
                Add(resource, new bn2());
            }
            
            foreach (EResource resource in genericProduction.Keys)
            {
                this[resource] = new bn2(genericProduction[resource]);
            }
        }

        public bn GetTotal()
        {
            bn result = new bn();
            foreach (EResource resource in Keys)
            {
                result += this[resource].Value;
            }

            return result;
        }

        public static Production operator *(Production a, bn b)
        {
            Production result = new Production(a);
            foreach (EResource resource in Enum.GetValues(typeof(EResource)))
            {
                result[resource].Multi *= b;
            }

            return result;
        }

        public override string ToString()
        {
            return new Resource(this).ToString();
        }

        [Serializable]
        public class SProductions : GenericDictionary<EResource, Vector3>{}
    }
    
    
    public static class ProductionsExtension
    {
        public static void Merge(this Production a, Production b)
        {
            foreach (EResource resource in a.Keys)
            {
                a[resource].Merge(b[resource]);
            }
        }
    
        public static void Merge(this Production a, List<Production> list)
        {
            foreach (EResource resource in a.Keys)
            {
                foreach (Production production in list)
                {
                    a[resource].Merge(production[resource]);
                }
            }
        }
    }
}