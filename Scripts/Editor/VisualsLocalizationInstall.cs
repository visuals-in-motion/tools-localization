using UnityEngine;
using System.Collections;
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
            string packagePath = GetPackageRelativePath();

            string json = File.ReadAllText(packagePath + "/package.json");
            package = JsonUtility.FromJson<Package>(json);

            if (package.version != LocalizationStorage.GetVersion())
            {
                LocalizationStorage.SetVersion(package.version);
                AssetDatabase.ImportPackage(packagePath + "/Package Resources/StreamingAssets.unitypackage", true);
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
