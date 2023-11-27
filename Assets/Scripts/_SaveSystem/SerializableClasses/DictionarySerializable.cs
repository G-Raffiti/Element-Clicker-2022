using System;
using System.Collections.Generic;
using _Extensions;
using UnityEngine;

namespace _SaveSystem.SerializableClasses
{
    [Serializable]
    public class DictionarySerializable<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<TKey> keys = new List<TKey>();
        [SerializeField] private List<TValue> values = new List<TValue>();

        // save the dictionary to lists
        public void OnBeforeSerialize()
        {
            keys = new List<TKey>();
            values = new List<TValue>();
            foreach (KeyValuePair<TKey, TValue> pair in this)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }

        // load the dictionary from lists
        public void OnAfterDeserialize()
        {
            this.Clear();

            if (keys.Count != values.Count)
            {
                Debug.LogError("Tried to deserialize a SerializableDictionary, but the amount of keys ("
                               + keys.Count + ") does not match the number of values (" + values.Count
                               + ") which indicates that something went wrong");
            }

            for (int i = 0; i < keys.Count; i++)
            {
                this.Add(keys[i], values[i]);
            }
        }

        public DictionarySerializable(Dictionary<TKey, TValue> dictionary)
        {
            keys = new List<TKey>();
            values = new List<TValue>();
            
            foreach (KeyValuePair<TKey, TValue> pair in dictionary)
            {
                this.Set(pair.Key, pair.Value);
            }
        }

        public Dictionary<TKey, TValue> GetDictionary(DictionarySerializable<TKey, TValue> dictionary)
        {
            Dictionary<TKey, TValue> result = new Dictionary<TKey, TValue>();
            foreach (KeyValuePair<TKey, TValue> pair in dictionary)
            {
                result.Set(pair.Key, pair.Value);
            }

            return result;
        }
    }
}