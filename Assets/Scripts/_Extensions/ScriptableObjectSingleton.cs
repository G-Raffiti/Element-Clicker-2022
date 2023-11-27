using UnityEngine;

namespace _Extensions
{
    // Singleton as Scriptable Object make the script reachable from anywhere 
    // but at the same time we can move Field in Unity : perfect for setting or for balance the game
    public class ScriptableObjectSingleton<T> : ScriptableObject where T : ScriptableObjectSingleton<T>
    {
        private static T s_instance;

        public static T Instance
        {
            get
            {
                if (s_instance == null)
                {
                    T[] assets = UnityEngine.Resources.LoadAll<T>("");
                    if (assets.Length < 1)
                    {
                        throw new System.Exception("Could not find Singleton SO of Type " + typeof(T).ToString());
                    }
                    else if (assets.Length > 1)
                    {
                        Debug.LogWarning("Multiple Instance of the Singleton SO of Type " + typeof(T).ToString() +" found in Resources");
                    }

                    s_instance = assets[0];
                }

                return s_instance;
            }
        }
    }
}