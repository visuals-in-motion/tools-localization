#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
namespace Visuals
{
    [CustomEditor(typeof(VisualsLocalizationAudio))]
    public class VisualsLocalizationAudioEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            VisualsLocalizationAudio component = (VisualsLocalizationAudio)target;

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