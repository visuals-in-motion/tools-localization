#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
namespace Visuals
{
    [CustomEditor(typeof(VisualsLocalizationText)), CanEditMultipleObjects]
    public class VisualsLocalizationTextEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            VisualsLocalizationText component = (VisualsLocalizationText)target;
            
            base.OnInspectorGUI();
            VisualsLocalizationEditor.InspectorUI(component, targets);

            component.SaveNewValues(component.localizationEnable, component.localizationCategory, component.localizationKeyName);

            if (GUI.changed) EditorUtility.SetDirty(target);
        }
    }
}
#endif