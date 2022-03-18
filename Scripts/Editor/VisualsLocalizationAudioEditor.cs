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
            VisualsLocalizationEditor.InspectorUI(component, targets);

            component.SaveNewValues(component.localizationEnable, component.localizationCategory, component.localizationKeyName);

            if (GUI.changed) EditorUtility.SetDirty(target);
        }
    }
}
#endif