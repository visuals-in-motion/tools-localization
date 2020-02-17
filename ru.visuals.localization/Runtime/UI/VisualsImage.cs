using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

namespace Visuals
{
    [AddComponentMenu("UI/Visuals Localization/Image")]
    public class VisualsImage : Image, IEditor
    {
        private List<Sprite> sprites = new List<Sprite>();

        public bool localizationEnable { get; set; } = true;
        public string localizationType { get; } = "sprite";
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

            sprites.Clear();

            for (int i = 0; i < LocalizationStorage.GetLanguages().Count; i++)
            {
                string path = Application.streamingAssetsPath + "/" + LocalizationStorage.GetLocalization()[localizationCategory].keys[localizationKey].item.value[i];
                if (File.Exists(path))
                {
                    byte[] spriteBytes = File.ReadAllBytes(path);
                    Texture2D texture2D = new Texture2D(2, 2);
                    texture2D.LoadImage(spriteBytes);
                    Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
                    sprites.Add(sprite);
                }
                else
                {
                    sprites.Add(null);
                }
            }
        }

        public void LocalizationChange(int index)
        {
            if (!localizationEnable) return;

            sprite = sprites[index];
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
