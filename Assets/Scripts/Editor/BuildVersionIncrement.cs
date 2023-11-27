using _Extensions;
using Singletons;
using UnityEditor;
using UnityEngine;
using static Singletons.Settings;

namespace Editor
{
    public class BuildVersionIncrement
    {
        const string SEPARATOR = ".";
        
        
        [MenuItem("Element/NextVersion")]
        static void IncrementVersion()
        {
            string[] version = ScriptableObjectSingleton<Settings>.Instance.BuildVersion.Split(SEPARATOR);
            if (version.Length < 3)
                version = new string[4]
                {
                    "00", "00", "a", "00"
                };
            string maj = version[0];
            string min = version[1];
            string patch = version[2];
            string build = version[3];

            int buildValue = int.Parse(build);
            buildValue++;
            build = "" + buildValue;

            string buildVersion = maj + SEPARATOR + min + SEPARATOR + patch + SEPARATOR + build;

            ScriptableObjectSingleton<Settings>.Instance.SetBuildVersion(buildVersion);
        }
    }
}