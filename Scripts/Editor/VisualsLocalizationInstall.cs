#if UNITY_EDITOR

using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Visuals
{
    [InitializeOnLoad]
    public class VisualsLocalizationInstall
    {
        private static string manifestPath;
        static VisualsLocalizationInstall()
        {
#if UNITY_EDITOR_WIN
            CheckDependencyInManifest();
            CheckCredentials();
#endif
        }
        [MenuItem("Visuals/Localization/Import Google Sheets")]
		public static void ImportGoogleSheets()
		{
#if GOOGLE_LIB && UNITY_EDITOR
            VisualsLocalization.Import();
#endif
        }
        [MenuItem("Visuals/Localization/Open Google Sheets")]
        public static void OpenGoogleSheets()
        {
            VisualsLocalization.Open();  
        }

        private static void CheckDependencyInManifest()
        {
            manifestPath = Path.GetFullPath("Packages/manifest.json");
            string googleLibrariesPackage = "    \"ru.visuals.google-libraries\": \"https://github.com/visuals-in-motion/tools-google-libraries.git\",";

            List<string> file = File.ReadAllLines(manifestPath).ToList();

            AddDependency(file, googleLibrariesPackage);
        }
        private static void AddDependency(List<string> file, string dependency)
        {
            if (!file.Contains(dependency))
            {
                file.Insert(2, dependency);
                File.WriteAllLines(manifestPath, file);
            }
        }
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
                string packagePath = GetPackageRelativePath();
                if (packagePath != null)
				{
                    File.Copy(packagePath + "/Package Resources/credentials.json", streamingPath + "/credentials.json");
                }
            }
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