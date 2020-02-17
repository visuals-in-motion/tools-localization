using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

namespace Visuals
{
    public class VisualsButtonEditor
    {
        [MenuItem("GameObject/UI/Visuals Localization/Button")]
        private static void CreateButton(MenuCommand menuCommand)
        {
            GameObject go = new GameObject("Button");
            RectTransform rectTransform = go.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(160, 30);
            go.AddComponent<CanvasRenderer>();
            VisualsImage visualsImage = go.AddComponent<VisualsImage>();
            visualsImage.raycastTarget = true;
            go.AddComponent<Button>();

            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }

        [MenuItem("GameObject/UI/Visuals Localization/Button - Text")]
        private static void CreateButtonText(MenuCommand menuCommand)
        {
            GameObject go = new GameObject("Button");
            RectTransform rectTransform = go.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(160, 30);
            go.AddComponent<CanvasRenderer>();
            VisualsImage visualsImage = go.AddComponent<VisualsImage>();
            visualsImage.raycastTarget = true;
            go.AddComponent<Button>();

            GameObject goText = new GameObject("Text");
            RectTransform rectTransformText = goText.AddComponent<RectTransform>();
            goText.transform.SetParent(go.transform);
            rectTransformText.anchorMin = new Vector2(0, 0);
            rectTransformText.anchorMax = new Vector2(1, 1);
            rectTransformText.offsetMin = new Vector2(0, 0);
            rectTransformText.offsetMax = new Vector2(-0, -0);
            goText.AddComponent<CanvasRenderer>();
            VisualsText visualsText = goText.AddComponent<VisualsText>();
            visualsText.raycastTarget = false;
            visualsText.alignment = TextAnchor.MiddleCenter;
            visualsText.color = new Color32(50, 50, 50, 255);

            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }

        [MenuItem("GameObject/UI/Visuals Localization/Button - TextMeshPro")]
        private static void CreateButtonTextMeshPro(MenuCommand menuCommand)
        {
            GameObject go = new GameObject("Button");
            RectTransform rectTransform = go.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(160, 30);
            go.AddComponent<CanvasRenderer>();
            VisualsImage visualsImage = go.AddComponent<VisualsImage>();
            visualsImage.raycastTarget = true;
            go.AddComponent<Button>();

            GameObject goText = new GameObject("Text (TMP)");            
            RectTransform rectTransformText = goText.AddComponent<RectTransform>();
            goText.transform.SetParent(go.transform);            
            rectTransformText.anchorMin = new Vector2(0, 0);
            rectTransformText.anchorMax = new Vector2(1, 1);
            rectTransformText.offsetMin = new Vector2(0, 0);
            rectTransformText.offsetMax = new Vector2(-0, -0);
            goText.AddComponent<CanvasRenderer>();
            VisualsTextMeshPro visualsTextMeshPro = goText.AddComponent<VisualsTextMeshPro>();
            visualsTextMeshPro.raycastTarget = false;
            visualsTextMeshPro.alignment = TMPro.TextAlignmentOptions.Center;
            visualsTextMeshPro.color = new Color32(50, 50, 50, 255);

            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }
    }
}