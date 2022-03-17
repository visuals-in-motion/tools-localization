#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Visuals
{
    [CustomEditor(typeof(VisualsLocalizationText))]
    public class VisualsLocalizationTextEditor : Editor
    {
		public override void OnInspectorGUI()
        {
            VisualsLocalizationText component = (VisualsLocalizationText)target;

            base.OnInspectorGUI();
            VisualsLocalizationEditor.InspectorUI(component);
            component.saveEnable = component.localizationEnable;
            component.saveCategory = component.localizationCategory;
            component.saveKeyName = component.localizationKeyName;

            if (GUI.changed) EditorUtility.SetDirty(target);
        }
    }
}
#endif