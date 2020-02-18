using UnityEngine;
using UnityEditor;
using System;
using System.IO;

namespace Visuals
{
    public class VisualsLocalizationEditor
    {
        [MenuItem("Visuals/Localization/Import StreamingAssets")]
        public static void CheckCredentials()
        {
            string path = Application.streamingAssetsPath + "/Localization";
            if (!File.Exists(path + "/credentials.json"))
            {
                string packagePath = Path.GetFullPath("Packages/ru.visuals.localization");
                Debug.LogError(packagePath);
                AssetDatabase.ImportPackage(packagePath + "/Package Resources/StreamingAssets.unitypackage", true);
            }
        }

        public static void InspectorUI(IEditor component)
        {
            //CheckCredentials();

            if (LocalizationStorage.GetCategories().Count > 0)
            {
                EditorGUILayout.Space();

                component.localizationEnable = EditorGUILayout.ToggleLeft("Visuals Localization", component.localizationEnable);
                if (component.localizationEnable)
                {
                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    component.localizationCategory = EditorGUILayout.Popup("Category", component.localizationCategory, LocalizationStorage.GetCategories().ToArray());

                    EditorGUI.BeginChangeCheck();
                    component.localizationKey = EditorGUILayout.Popup("Key", component.localizationKey, LocalizationStorage.GetKeys(component.localizationCategory).ToArray());
                    if (EditorGUI.EndChangeCheck())
                    {
                        component.LocalizationLoad();
                    }

                    EditorGUILayout.Space();

                    EditorGUI.BeginChangeCheck();
                    int language = EditorGUILayout.Popup("Language", LocalizationStorage.GetCurrentLanguages(), LocalizationStorage.GetLanguages().ToArray());
                    if (EditorGUI.EndChangeCheck())
                    {
                        LocalizationStorage.SetCurrentLanguages(language);
                    }

                    EditorGUILayout.Space();

                    if (GUILayout.Button("Import"))
                    {
                        VisualsLocalization.Import();
                    }

                    EditorGUILayout.Space();

                    if (GUILayout.Button("Export"))
                    {
                        VisualsLocalization.Export();
                    }

                    EditorGUILayout.Space();

                    if (GUILayout.Button("Open sheets"))
                    {
                        Application.OpenURL("https://docs.google.com/spreadsheets/d/" + LocalizationStorage.GetSpreadsheetId());
                    }
                    GUILayout.EndHorizontal();

                    if (LocalizationStorage.CheckType(component.localizationCategory, component.localizationKey, component.localizationType))
                    {
                        component.LocalizationChange(LocalizationStorage.GetCurrentLanguages());
                    }
                }
            }
        }
    }
}