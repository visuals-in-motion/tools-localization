#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Visuals
{
    [CustomEditor(typeof(VisualsLocalizationImage))]
    public class VisualsLocalizationImageEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            VisualsLocalizationImage component = (VisualsLocalizationImage)target;

            base.OnInspectorGUI();
            VisualsLocalizationEditor.InspectorUI(component, targets);

            component.SaveNewValues(component.localizationEnable, component.localizationCategory, component.localizationKeyName);

            if (GUI.changed) EditorUtility.SetDirty(target);
        }
    }
}
#endif