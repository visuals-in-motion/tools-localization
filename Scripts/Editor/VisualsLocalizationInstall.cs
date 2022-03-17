#if UNITY_EDITOR

using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;

namespace Visuals
{
    [InitializeOnLoad]
    public class VisualsLocalizationInstall
    {
        private static string manifestPath;
        static VisualsLocalizationInstall()
        {
#if UNITY_EDITOR_WIN
            CheckDependencyInManifest(); //
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
    }
}
#endif