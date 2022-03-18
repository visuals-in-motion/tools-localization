#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
namespace Visuals
{
    [CustomEditor(typeof(VisualsLocalizationRawImage))]
    public class VisualsLocalizationRawImageEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            VisualsLocalizationRawImage component = (VisualsLocalizationRawImage)target;

            base.OnInspectorGUI();
            VisualsLocalizationEditor.InspectorUI(component, targets);

            component.SaveNewValues(component.localizationEnable, component.localizationCategory, component.localizationKeyName);

            if (GUI.changed) EditorUtility.SetDirty(target);
        }
    }
}
#endif