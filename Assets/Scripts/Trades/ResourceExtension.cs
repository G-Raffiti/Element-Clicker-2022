using System.Collections.Generic;
using _Extensions;
using _SaveSystem.SerializableClasses;
using BigNumbers;

namespace Trades
{
    public static class ResourceExtension
    {
        public static Resource GetResource(this ResourceSerializable resourceS)
        {
            Resource result = new Resource();
            foreach (KeyValuePair<EResource, bn> pair in resourceS)
            {
                result.Set(pair.Key, pair.Value);
            }

            return result;
        }
    }
}