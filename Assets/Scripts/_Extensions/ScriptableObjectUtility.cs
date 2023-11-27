using System.Collections;
using UnityEngine;

namespace _Extensions
{
    /// <summary>
    /// class ScriptableObjectUtility enable ScriptableObjects to Start Coroutine
    /// </summary>
    public class ScriptableObjectUtility : MonoBehaviour
    {
        private static ScriptableObjectUtility instance;

        private void Start()
        {
            instance = this;
        }

        public static void StartsCoroutine(IEnumerator routine)
        {
            instance.StartCoroutine(routine);
        }


        
    }
}