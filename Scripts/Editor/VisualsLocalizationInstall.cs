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

        [MenuItem("Visuals/Localization/Import StreamingAssets")]
        public static void CheckCredentials()
        {
            string packagePath = GetPackageRelativePath();
            AssetDatabase.ImportPackage(packagePath + "/Package Resources/StreamingAssets.unitypackage", true);
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
