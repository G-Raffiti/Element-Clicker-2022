using System;
using System.Collections.Generic;
using _Extensions;
using BigNumbers;
using Trades;

namespace _SaveSystem.SerializableClasses
{
    [Serializable]
    public class ResourceSerializable : DictionarySerializable<EResource, bn>
    {
        public ResourceSerializable(Resource resource) : base(resource)
        {
            foreach (KeyValuePair<EResource, bn> pair in resource)
            {
                this.Set(pair.Key, pair.Value);
            }
        }
    }
}