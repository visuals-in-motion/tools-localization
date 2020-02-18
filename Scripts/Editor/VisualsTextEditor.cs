using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Visuals
{
    [CustomEditor(typeof(VisualsText))]
    //[CanEditMultipleObjects]
    public class VisualsTextEditor : UnityEditor.UI.TextEditor
    {
        [MenuItem("GameObject/UI/Visuals Localization/Text")]
        private static void CreateText(MenuCommand menuCommand)
        {
            GameObject go = new GameObject("Text");
            RectTransform rectTransform = go.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(160, 30);
            go.AddComponent<CanvasRenderer>();
            VisualsText visualsText = go.AddComponent<VisualsText>();
            visualsText.text = "New Text";
            visualsText.raycastTarget = false;

            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }

        public override void OnInspectorGUI()
        {
            VisualsText component = (VisualsText)target;

            base.OnInspectorGUI();

            VisualsLocalizationEditor.InspectorUI(component);

            component.saveEnable = component.localizationEnable;
            component.saveCategory = component.localizationCategory;
            component.saveKey = component.localizationKey;

            if (GUI.changed) EditorUtility.SetDirty(target);
        }
    }
}
