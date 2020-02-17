using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEditor;
using UnityEngine;

namespace Visuals
{
    [CustomEditor(typeof(VisualsTextMeshPro))]
    //[CanEditMultipleObjects]
    public class VisualsTextMeshProEditor : TMP_EditorPanel
    {
        [MenuItem("GameObject/UI/Visuals Localization/Text - TextMeshPro")]
        private static void CreateText(MenuCommand menuCommand)
        {
            GameObject go = new GameObject("Text (TMP)");
            RectTransform rectTransform = go.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(200, 50);
            go.AddComponent<CanvasRenderer>();
            VisualsTextMeshPro visualsTextMeshPro = go.AddComponent<VisualsTextMeshPro>();
            visualsTextMeshPro.text = "New Text";
            visualsTextMeshPro.raycastTarget = false;

            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }

        public override void OnInspectorGUI()
        {
            VisualsTextMeshPro component = (VisualsTextMeshPro)target;

            base.OnInspectorGUI();

            VisualsLocalizationEditor.InspectorUI(component);

            component.saveEnable = component.localizationEnable;
            component.saveCategory = component.localizationCategory;
            component.saveKey = component.localizationKey;

            if (GUI.changed) EditorUtility.SetDirty(target);
        }
    }
}
