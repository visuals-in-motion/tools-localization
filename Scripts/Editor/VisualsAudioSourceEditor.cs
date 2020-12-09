#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Visuals
{
    [CustomEditor(typeof(VisualsAudioSource))]
    public class VisualsAudioSourceEditor : Editor
    {
        [MenuItem("GameObject/Audio/Visuals Localization/Audio Source")]
        private static void CreateText(MenuCommand menuCommand)
        {
            GameObject go = new GameObject("Audio Source");
            go.AddComponent<AudioSource>();
            go.AddComponent<VisualsAudioSource>();

            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }

        public override void OnInspectorGUI()
        {
            VisualsAudioSource component = (VisualsAudioSource)target;

            base.OnInspectorGUI();

            VisualsLocalizationEditor.InspectorUI(component);

            component.saveEnable = component.localizationEnable;
            component.saveCategory = component.localizationCategory;
            component.saveKey = component.localizationKey;

            if (GUI.changed) EditorUtility.SetDirty(target);
        }
    }
}
#endif