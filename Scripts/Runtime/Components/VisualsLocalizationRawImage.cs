using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
namespace Visuals
{
    [ExecuteAlways]
    public class VisualsLocalizationRawImage : VisualsLocalizationComponent
    {
        private RawImage rawImageComponent = null;
        private Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        public override string localizationType { get; } = "texture";

        protected override void Enable()
        {
            rawImageComponent = GetComponent<RawImage>();
        }

        public override void LocalizationLoad()
        {
            if (!localizationEnable) return;

            textures.Clear();

            for (int i = 0; i < LocalizationStorage.GetLanguages().Count; i++)
            {
                string fileName = LocalizationStorage.GetLocalization()[localizationCategoryIndex].keys[localizationKey].value[i];
                string path = Application.streamingAssetsPath + "/" + fileName;
                if (File.Exists(path))
                {
                    byte[] textureBytes = File.ReadAllBytes(path);
                    Texture2D texture2D = new Texture2D(2, 2);
                    texture2D.LoadImage(textureBytes);
                    textures.Add(fileName, texture2D);
                }
            }
        }
        protected override void SetValues(string value)
        {
            if (rawImageComponent != null)
            {
                if (textures.ContainsKey(value))
                {
                    rawImageComponent.texture = textures[value];
                }
            }
        }
    }
}