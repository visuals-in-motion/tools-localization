#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Visuals
{
    class VisualsPanelLanguageEditor
    {
        [MenuItem("GameObject/UI/Visuals Localization/Panel Language Horizontal")]
        private static void CreatePanelLanguageHorizontal(MenuCommand menuCommand)
        {
            GameObject go = new GameObject("PanelLanguage");
            RectTransform rectTransform = go.AddComponent<RectTransform>();
            HorizontalLayoutGroup horizontalLayoutGroup = go.AddComponent<HorizontalLayoutGroup>();
            horizontalLayoutGroup.childControlWidth = true;
            horizontalLayoutGroup.childControlHeight = true;
            horizontalLayoutGroup.childScaleWidth = true;
            horizontalLayoutGroup.childScaleHeight = true;
            horizontalLayoutGroup.childForceExpandWidth = true;
            horizontalLayoutGroup.childForceExpandHeight = true;
            ContentSizeFitter contentSizeFitter = go.AddComponent<ContentSizeFitter>();
            contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            for (int i = 0; i < LocalizationStorage.GetLanguages().Count; i++)
            {
                CreateButton(go.transform, i);
            }

            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }

        [MenuItem("GameObject/UI/Visuals Localization/Panel Language Vertical")]
        private static void CreatePanelLanguageVertical(MenuCommand menuCommand)
        {
            GameObject go = new GameObject("PanelLanguage");
            RectTransform rectTransform = go.AddComponent<RectTransform>();
            VerticalLayoutGroup verticalLayoutGroup = go.AddComponent<VerticalLayoutGroup>();
            verticalLayoutGroup.childControlWidth = true;
            verticalLayoutGroup.childControlHeight = true;
            verticalLayoutGroup.childScaleWidth = true;
            verticalLayoutGroup.childScaleHeight = true;
            verticalLayoutGroup.childForceExpandWidth = true;
            verticalLayoutGroup.childForceExpandHeight = true;
            ContentSizeFitter contentSizeFitter = go.AddComponent<ContentSizeFitter>();
            contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            for (int i = 0; i < LocalizationStorage.GetLanguages().Count; i++)
            {
                CreateButton(go.transform, i);
            }

            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }

        private static void CreateButton(Transform parent, int index)
        {
            GameObject go = new GameObject("Button"+LocalizationStorage.GetLanguages()[index]);
            RectTransform rectTransform = go.AddComponent<RectTransform>();
            go.transform.SetParent(parent);
            go.AddComponent<CanvasRenderer>();
            go.AddComponent<Image>();
            VisualsButtonLanguage visualsButtonLanguage = go.AddComponent<VisualsButtonLanguage>();
            visualsButtonLanguage.language = index;
        }
    }
}
#endif