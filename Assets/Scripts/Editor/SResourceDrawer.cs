using BigNumbers;
using Trades;
using UnityEditor;
using UnityEngine;

namespace _Extensions
{
    [CustomPropertyDrawer(typeof(Resource.SResource))]
    public class SResourceDrawer : DictionaryDrawer<EResource, Vector2>
    {
    }
}