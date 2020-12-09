using TMPro;
using UnityEngine;

namespace Visuals
{
    [AddComponentMenu("UI/Visuals Localization/TextMeshPro - Text (UI)")]
    public class VisualsTextMeshPro : TextMeshProUGUI, IEditor
    {
        public bool localizationEnable { get; set; } = true;
        public string localizationType { get; } = "text";
        public int localizationCategory { get; set; } = 0;
        public int localizationKey { get; set; } = 0;

        public bool saveEnable = true;
        public int saveCategory = 0;
        public int saveKey = 0;

        protected override void Awake()
        {
            base.Awake();

            localizationEnable = saveEnable;
            localizationCategory = saveCategory;
            localizationKey = saveKey;
        }

        public void LocalizationLoad()
        {
            if (!localizationEnable) return;
        }

        public void LocalizationChange(int index)
        {
            if (!localizationEnable) return;

            text = LocalizationStorage.GetLocalization()[localizationCategory].keys[localizationKey].item.value[index];
        }

        private void Update()
        {
            if (!localizationEnable) return;

            if (LocalizationStorage.CheckType(localizationCategory, localizationKey, localizationType))
            {
                LocalizationChange(LocalizationStorage.GetCurrentLanguages());
            }
        }
    }
}
