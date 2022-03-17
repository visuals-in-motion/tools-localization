using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

namespace Visuals
{
    [ExecuteAlways]
    public class VisualsLocalizationImage : VisualsLocalizationComponent
    {
        private Image imageComponent = null;
        private Dictionary<string,Sprite> sprites = new Dictionary<string, Sprite>();
        public override string localizationType { get; } = "sprite";
        protected override void Enable()
        {
            imageComponent = GetComponent<Image>();
        }
        public override void LocalizationChange(int languageIndex, bool changeValues)
        {
            if (!localizationEnable) return;

            localizationKey = GetKeyIndexByName(localizationKeyName);
            
            List<string> values = GetValuesByKeyIndex(localizationKey);
           
            if (values != null)
            {
                SetValues(values[languageIndex]);
                if (changeValues)
                {
                    LocalizationLoad();
                }
            }
        }
        public override void LocalizationLoad()
        {
            if (!localizationEnable) return;
            localizationKey = GetKeyIndexByName(localizationKeyName);
            sprites.Clear();
			for (int i = 0; i < LocalizationStorage.GetLanguages().Count; i++)
			{
                List<LocalizationStorage.KeyItem> keys = LocalizationStorage.GetLocalization()[localizationCategoryIndex].keys;
                
                if (localizationKey >= 0 && localizationKey < keys.Count)
				{
                    string fileName = keys[localizationKey].value[i];

                    string path = Application.streamingAssetsPath + "/" + fileName;
                    if (File.Exists(path))
                    {
                        byte[] spriteBytes = File.ReadAllBytes(path);
                        Texture2D texture2D = new Texture2D(2, 2);
                        texture2D.LoadImage(spriteBytes);
                        Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
                        sprites.Add(fileName, sprite);
                    }
                }
			}
		}
        protected override void SetValues(string value)
        {
            if(imageComponent != null)
			{
                if(sprites.Count == 0)
				{
                    LocalizationLoad();
                }
                if(sprites.ContainsKey(value))
				{
                    imageComponent.sprite = sprites[value];
                } 
            }
        }
    }
}
