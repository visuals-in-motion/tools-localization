#if UNITY_EDITOR
using UnityEditor;

namespace Visuals
{
    [CustomEditor(typeof(VisualsButtonLanguage))]
    public class VisualsButtonLanguageEditor : UnityEditor.UI.ButtonEditor
    {
        public override void OnInspectorGUI()
        {
            VisualsButtonLanguage component = (VisualsButtonLanguage)target;

            base.OnInspectorGUI();

            if (LocalizationStorage.GetCurrentLanguages() == component.language)
            {
                component.image.sprite = component.languageOn;
            }
            else
            {
                component.image.sprite = component.languageOff;
            }
        }
    }
}
#endif