using UnityEngine;
using UnityEditor;

namespace Visuals
{
    [CustomEditor(typeof(VisualsRawImage))]
    public class VisualsRawImageEditor : UnityEditor.UI.RawImageEditor
    {
        [MenuItem("GameObject/UI/Visuals Localization/RawImage")]
        private static void CreateRawImage(MenuCommand menuCommand)
        {
            GameObject go = new GameObject("RawImage");
            RectTransform rectTransform = go.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(100, 100);
            go.AddComponent<CanvasRenderer>();
            VisualsRawImage visualsImage = go.AddComponent<VisualsRawImage>();
            visualsImage.raycastTarget = false;

            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }

        public override void OnInspectorGUI()
        {
            VisualsRawImage component = (VisualsRawImage)target;

            base.OnInspectorGUI();

            VisualsLocalizationEditor.InspectorUI(component);

            component.saveEnable = component.localizationEnable;
            component.saveCategory = component.localizationCategory;
            component.saveKey = component.localizationKey;

            if (GUI.changed) EditorUtility.SetDirty(target);
        }
    }
}