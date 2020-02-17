using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

namespace Visuals
{
    [AddComponentMenu("UI/Visuals Localization/RawImage")]
    public class VisualsRawImage : RawImage, IEditor
    {
        private List<Texture2D> textures = new List<Texture2D>();

        public bool localizationEnable { get; set; } = true;
        public string localizationType { get; } = "texture";
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

        protected override void Start()
        {
            base.Start();

            LocalizationLoad();
        }

        public void LocalizationLoad()
        {
            if (!localizationEnable) return;

            textures.Clear();

            for (int i = 0; i < LocalizationStorage.GetLanguages().Count; i++)
            {
                string path = Application.streamingAssetsPath + "/" + LocalizationStorage.GetLocalization()[localizationCategory].keys[localizationKey].item.value[i];
                if (File.Exists(path))
                {
                    byte[] textureBytes = File.ReadAllBytes(path);
                    Texture2D texture2D = new Texture2D(2, 2);
                    texture2D.LoadImage(textureBytes);
                    textures.Add(texture2D);
                }
                else
                {
                    textures.Add(null);
                }
            }
        }

        public void LocalizationChange(int index)
        {
            if (!localizationEnable) return;

            texture = textures[index];
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
