using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

namespace Visuals
{
    public class VisualsLocalizationEditor
    {
        static int localizationKeyIndex = 0;
        static int localizationCategoryIndex = 0;
        public static void InspectorUI(VisualsLocalizationComponent component, Object[] targets)
        {
            if (LocalizationStorage.GetCategories() != null && LocalizationStorage.GetCategories().Count > 0)
            {
                EditorGUILayout.Space();

                component.localizationEnable = EditorGUILayout.ToggleLeft("Visuals Localization", component.localizationEnable);
                if (component.localizationEnable)
                {
                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    EditorGUI.BeginChangeCheck();

                    List<string> categoriesByType = LocalizationStorage.GetCategoriesWithKeysType(component.localizationType);
                    localizationCategoryIndex = categoriesByType.IndexOf(component.localizationCategory);
                    localizationCategoryIndex = EditorGUILayout.Popup("Category", localizationCategoryIndex, categoriesByType.ToArray());

                    if (localizationCategoryIndex == -1 || categoriesByType.Count == 0)
					{
                        localizationCategoryIndex = 0;
                        
                        if(categoriesByType.Count == 0)
						{
                            component.localizationCategory = LocalizationStorage.GetCategories()[0];
                        }
						else
						{
                            component.localizationCategory = categoriesByType[0];
                        }
                    }
					else
					{
                        if (categoriesByType.Count == 0)
                        {
                            component.localizationCategory = LocalizationStorage.GetCategories()[0];
                        }
                        else
                        {
                            component.localizationCategory = categoriesByType[localizationCategoryIndex];
                        }
                    }

                    List<string> keysByType = component.GetCurrentCategoryKeysByType(component.localizationType);
                    localizationKeyIndex = keysByType.IndexOf(component.localizationKeyName);
                    localizationKeyIndex = EditorGUILayout.Popup("Key", localizationKeyIndex, keysByType.ToArray());
                    if (localizationKeyIndex == -1)
                    {
                        localizationKeyIndex = 0;
                        if(keysByType.Count == 0)
						{
                            component.localizationKeyName = component.GetCurrentCategoryAllKeys()[0];
						}
						else
						{
                            component.localizationKeyName = keysByType[0];
                        }
					}
					else
					{
                        if (keysByType.Count == 0)
                        {
                            component.localizationKeyName = component.GetCurrentCategoryAllKeys()[0];
                        }
                        else
                        {
                            component.localizationKeyName = keysByType[localizationKeyIndex];
                        }
                    }
                    
                    if (EditorGUI.EndChangeCheck())
                    {
                        component.LocalizationLoad();

                        for (int i = 0; i < targets.Length; i++)
                        {
                            VisualsLocalizationComponent targetsComponent = (VisualsLocalizationComponent)targets[i];
                            targetsComponent.localizationEnable = component.localizationEnable;
                            targetsComponent.localizationCategory = component.localizationCategory;
                            targetsComponent.localizationKeyName = component.localizationKeyName;

                            targetsComponent.SaveNewValues(component.localizationEnable, component.localizationCategory, component.localizationKeyName);
                        }
                    }

                    EditorGUILayout.Space();

                    EditorGUI.BeginChangeCheck();
                    int lang = EditorGUILayout.Popup("Language", LocalizationStorage.GetCurrentLanguages(), LocalizationStorage.GetLanguages().ToArray());
                    if (EditorGUI.EndChangeCheck())
                    {
                        LocalizationStorage.SetCurrentLanguages(lang);
                    }

                    GUILayout.EndHorizontal();

                    if (LocalizationStorage.CheckType(component.localizationCategory, component.localizationKeyName, component.localizationType))
                    {
                        component.LocalizationChange(LocalizationStorage.GetCurrentLanguages(), false);
                    }
                }
            }
        }
    }
}
#endif