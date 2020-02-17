using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Visuals
{
    public class VisualsButtonLanguage : Button
    {
        public int language = 0;
        public Sprite languageOn = null;
        public Sprite languageOff = null;

        protected override void Start()
        {
            base.Start();

            onClick.AddListener(() => LocalizationStorage.SetCurrentLanguages(language));

            string pathSpriteOn = Application.streamingAssetsPath + "/Localization/Language/On/" + language + ".png";
            if (File.Exists(pathSpriteOn))
            {
                byte[] spriteBytesOn = File.ReadAllBytes(pathSpriteOn);
                Texture2D texture2DOn = new Texture2D(2, 2);
                texture2DOn.LoadImage(spriteBytesOn);
                languageOn = Sprite.Create(texture2DOn, new Rect(0, 0, texture2DOn.width, texture2DOn.height), new Vector2(0.5f, 0.5f));
            }

            string pathSprite = Application.streamingAssetsPath + "/Localization/Language/Off/" + language + ".png";
            if (File.Exists(pathSprite))
            {
                byte[] spriteBytesOff = File.ReadAllBytes(pathSprite);
                Texture2D texture2DOff = new Texture2D(2, 2);
                texture2DOff.LoadImage(spriteBytesOff);
                languageOff = Sprite.Create(texture2DOff, new Rect(0, 0, texture2DOff.width, texture2DOff.height), new Vector2(0.5f, 0.5f));
            }
        }

        private void Update()
        {
            if (LocalizationStorage.GetCurrentLanguages() == language)
            {
                image.sprite = languageOn;
            }
            else
            {
                image.sprite = languageOff;
            }
        }
    }
}
