using System.Collections.Generic;
using System.Linq;
using BigNumbers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Extensions
{
    public static class Extension
    {
        /// <summary>
        /// Re Order the given List in a Random Order
        /// </summary>
        public static void Shuffle<T>(this IList<T> _list)
        {
            int count = _list.Count;
            int last = count - 1;
            for (int i = 0; i < last; i++)
            {
                int randomIndex = Random.Range(i, count);
                T tempValue = _list[i];
                _list[i] = _list[randomIndex];
                _list[randomIndex] = tempValue;
            }
        }

        /// <summary>
        /// Return a Random element of the Given List
        /// </summary>
        public static T GetRandom<T>(this IList<T> _list)
        {
            int max = _list.Count;
            return _list[Random.Range(0, max)];
        }

        /// <summary>
        /// Return the Key which has the highest Value of the given Dictionnary
        /// </summary>
        public static T GetKeyOfMaxValue<T>(this IDictionary<T, int> _dictionary)
        {
            T max = _dictionary.First().Key;
            foreach (KeyValuePair<T, int> pair in _dictionary)
            {
                if (pair.Value > _dictionary[max]) max = pair.Key;
            }

            return max;
        }
        
        /// <summary>
        /// Return the Key which has the highest Value of the given Dictionnary
        /// </summary>
        public static T GetKeyOfMaxValue<T>(this IDictionary<T, float> _dictionary)
        {
            T max = _dictionary.First().Key;
            foreach (KeyValuePair<T, float> pair in _dictionary)
            {
                if (pair.Value > _dictionary[max]) max = pair.Key;
            }

            return max;
        }
        
        /// <summary>
        /// Return the Key which has the lowest Value of the given Dictionnary
        /// </summary>
        public static T GetKeyOfMinValue<T>(this IDictionary<T, int> _dictionary)
        {
            T max = _dictionary.First().Key;
            foreach (KeyValuePair<T, int> pair in _dictionary)
            {
                if (pair.Value < _dictionary[max]) max = pair.Key;
            }

            return max;
        }
        
        /// <summary>
        /// Return the Key which has the lowest Value of the given Dictionnary
        /// </summary>
        public static T GetKeyOfMinValue<T>(this IDictionary<T, float> _dictionary)
        {
            T max = _dictionary.First().Key;
            foreach (KeyValuePair<T, float> pair in _dictionary)
            {
                if (pair.Value < _dictionary[max]) max = pair.Key;
            }

            return max;
        }
        
        /// <summary>
        /// Return the Key which has the lowest Value of the given Dictionnary
        /// </summary>
        public static T GetKeyOfMinValue<T>(this IDictionary<T, bn> _dictionary)
        {
            T max = _dictionary.First().Key;
            foreach (KeyValuePair<T, bn> pair in _dictionary)
            {
                if (pair.Value < _dictionary[max]) max = pair.Key;
            }

            return max;
        }

        public static void Set<TKey, TValue>(this Dictionary<TKey, TValue> _dictionary, TKey key, TValue value)
        {
            if (!_dictionary.ContainsKey(key))
            {
                _dictionary.Add(key, value);
            }
            else
            {
                _dictionary[key] = value;
            }
        }

        /// <summary>
        /// Destroy Immediatly all GameObject children of the Transform
        /// </summary>
        /// <param name="_transform"></param>
        public static void Clear(this Transform _transform)
        {
            while (_transform.childCount > 0)
            {
                GameObject.DestroyImmediate(_transform.GetChild(0).gameObject);
            }
        }

        /// <summary>
        /// Return True if the GameObjet has the Component T attached to him
        /// </summary>
        public static bool HasComponent<T>(this GameObject obj)
        {
            return obj.GetComponent<T>() != null;
        }
        
        /// <summary>
        /// Return True if the any of the GameObject chlid has the Component T attached to him
        /// </summary>
        public static bool HasComponentInChildren<T>(this GameObject obj)
        {
            return obj.GetComponentInChildren<T>() != null;
        }
    }
}