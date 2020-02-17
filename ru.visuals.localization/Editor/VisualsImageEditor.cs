using UnityEngine;
using UnityEditor;

namespace Visuals
{
    [CustomEditor(typeof(VisualsImage))]
    public class VisualsImageEditor : UnityEditor.UI.ImageEditor
    {
        [MenuItem("GameObject/UI/Visuals Localization/Image")]
        private static void CreateImage(MenuCommand menuCommand)
        {
            GameObject go = new GameObject("Image");
            RectTransform rectTransform = go.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(100, 100);
            go.AddComponent<CanvasRenderer>();
            VisualsImage visualsImage = go.AddComponent<VisualsImage>();
            visualsImage.raycastTarget = false;

            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }

        public override void OnInspectorGUI()
        {
            VisualsImage component = (VisualsImage)target;

            base.OnInspectorGUI();

            VisualsLocalizationEditor.InspectorUI(component);

            component.saveEnable = component.localizationEnable;
            component.saveCategory = component.localizationCategory;
            component.saveKey = component.localizationKey;

            if (GUI.changed) EditorUtility.SetDirty(target);
        }
    }
}