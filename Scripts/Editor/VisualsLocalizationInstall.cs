#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Visuals
{
    [InitializeOnLoad]
    public class VisualsLocalizationInstall
    {
        static VisualsLocalizationInstall()
        {
            CheckCredentials();
        }

        private static Package package;
        [SerializeField]
        private struct Package
        {
            public string name;
            public string displayName;
            public string version;
            public string unity;
            public string author;
        }

        [MenuItem("Visuals/Localization/Import StreamingAssets")] 
        public static void CheckCredentials() 
        {
            if (!Directory.Exists(Application.dataPath + "/Resources")) Directory.CreateDirectory(Application.dataPath + "/Resources");
            if (!File.Exists(Application.dataPath + "/Resources/Localization.asset"))
            {
                LocalizationStorage asset = ScriptableObject.CreateInstance<LocalizationStorage>();
                AssetDatabase.CreateAsset(asset, "Assets/Resources/Localization.asset"); 
                AssetDatabase.SaveAssets();
            }

            string streamingPath = Application.streamingAssetsPath + "/Localization";
            if (!File.Exists(streamingPath + "/credentials.json")) 
            {
                if (!Directory.Exists(streamingPath)) Directory.CreateDirectory(streamingPath);
                File.Copy(GetPackageRelativePath() + "/Package Resources/credentials.json", streamingPath + "/credentials.json");
            }
        }

        [MenuItem("Visuals/Localization/Import Google Sheets")]
        public static void ImportGoogleSheets()
        {
            VisualsLocalization.Import();
        }

        private static string GetPackageRelativePath()
        {
            string packagePath = Path.GetFullPath("Packages/ru.visuals.localization");
            if (Directory.Exists(packagePath))
            {
                return packagePath;
            }

            packagePath = Path.GetFullPath("Assets/..");
            if (Directory.Exists(packagePath))
            {
                packagePath = packagePath + "/Assets/Packages/ru.visuals.localization";
                if (Directory.Exists(packagePath))
                {
                    return packagePath;
                }
            }

            Debug.LogError("Error: path not found");
            return null;
        }
    }
}
#endif