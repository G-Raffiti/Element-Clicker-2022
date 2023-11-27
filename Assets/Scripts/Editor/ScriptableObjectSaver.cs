using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CreateAssetMenu(fileName = "Utility_Saver", menuName = "Scriptable Object/Utility/Saver")]
    public class ScriptableObjectSaver : ScriptableObject
    {
        [SerializeField] private List<ScriptableObject> All;

#if (UNITY_EDITOR)
        [ContextMenu("Save ScriptableObject")]
        void SetAllDirty()
        {
            foreach (ScriptableObject scriptableObject in All)
            {
                EditorUtility.SetDirty(scriptableObject);
            }
        }
#endif
    }
}