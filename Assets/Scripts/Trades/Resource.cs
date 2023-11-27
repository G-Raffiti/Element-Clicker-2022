using System;
using System.Collections.Generic;
using _Extensions;
using BigNumbers;
using Singletons;
using UnityEngine;

namespace Trades
{
    public enum EResource
    {
        Gold,
        Nature,
        Water,
        Fire,
        Earth,
        Air,
        Time,
        Rage,
        Tech,
        Ether,
        Mana,
        Click,
    }

    public class Resource : Dictionary<EResource, bn>
    {
        public Resource()
        {
            foreach (EResource resource in Enum.GetValues(typeof(EResource)))
            {
                Add(resource, 0);
            }
        }

        public Resource(SResource sResource)
        {
            foreach (EResource res in Enum.GetValues(typeof(EResource)))
            {
                Add(res, 0);
            }
            
            foreach (EResource resource in sResource.Keys)
            {
                this[resource] = sResource[resource];
            }
        }

        public Resource(Resource a)
        {
            foreach (EResource res in Enum.GetValues(typeof(EResource)))
            {
                Add(res, 0);
            }
            
            foreach (EResource resource in a.Keys)
            {
                this[resource] = a[resource];
            }
        }

        public Resource(Production production)
        {
            foreach (EResource res in Enum.GetValues(typeof(EResource)))
            {
                Add(res, 0);
            }
            foreach (EResource resource in production.Keys)
            {
                this[resource] = production[resource].Value;
            }
        }

        public bn GetTotal()
        {
            bn result = new bn();
            foreach (EResource resource in Keys)
            {
                result += this[resource];
            }

            return result;
        }
    
        public static Resource operator +(Resource a, Resource b)
        {
            Resource result = new Resource();
            foreach (EResource resource in Enum.GetValues(typeof(EResource)))
            {
                result[resource] = a[resource] + b[resource];
            }
            return result;
        }
    
        public static Resource operator -(Resource a, Resource b)
        {
            Resource result = new Resource();
            foreach (EResource resource in Enum.GetValues(typeof(EResource)))
            {
                result[resource] = a[resource] - b[resource];
            }
            return result;
        }

        public static Resource operator *(Resource a, float factor)
        {
            Resource result = new Resource(a);
            foreach (EResource resource in a.Keys)
            {
                result[resource] *= factor;
            }

            return result;
        }
        
        public static Resource operator *(Resource a, bn factor)
        {
            Resource result = new Resource(a);
            foreach (EResource resource in a.Keys)
            {
                result[resource] *= factor;
            }

            return result;
        }
        
        public static bool operator >(Resource a, Resource b)
        {
            foreach (EResource resource in a.Keys)
            {
                if (a[resource] <= b[resource]) return false;
            }

            return true;
        }

        public static bool operator <(Resource a, Resource b)
        {
            foreach (EResource resource in a.Keys)
            {
                if (a[resource] >= b[resource]) return false;
            }

            return true;
        }

        public static bool operator ==(Resource a, Resource b)
        {
            foreach (EResource resource in a.Keys)
            {
                if (a[resource] != b[resource]) return false;
            }

            return true;
        }

        public static bool operator !=(Resource a, Resource b)
        {
            foreach (EResource resource in a.Keys)
            {
                if (a[resource] == b[resource]) return false;
            }

            return true;
        }

        public static bool operator >=(Resource a, Resource b)
        {
            foreach (EResource resource in a.Keys)
            {
                if (a[resource] < b[resource]) return false;
            }

            return true;
        }

        public static bool operator <=(Resource a, Resource b)
        {
            foreach (EResource resource in a.Keys)
            {
                if (a[resource] > b[resource]) return false;
            }

            return true;
        }
    
        public bool Equals(Resource other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Resource) obj);
        }

        public override int GetHashCode()
        {
            return (int)(GetTotal().Exp * GetTotal().Value);
        }

        public override string ToString()
        {
            string str = "";
            foreach (EResource resource in Keys)
            {
                if (this[resource] != 0)
                {
                    str += $"{this[resource].ToString(Settings.Instance.Format)} {resource.String()}\n";
                }
            }

            return str;
        }

        [Serializable]
        public class SResource : GenericDictionary<EResource, Vector2> {}
    }
}