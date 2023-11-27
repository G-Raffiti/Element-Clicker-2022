using BigNumbers;
using Trades;
using UnityEditor;
using UnityEngine;

namespace _Extensions
{
    [CustomPropertyDrawer(typeof(Production.SProductions))]
    public class SProductionDrawer : DictionaryDrawer<EResource, Vector3>
    {
    }
}