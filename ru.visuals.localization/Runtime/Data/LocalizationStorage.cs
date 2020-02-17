﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Visuals
{
    [CreateAssetMenu(fileName = "Localization", menuName = "Visuals/Localization")]
    [Serializable]
    public class LocalizationStorage : ScriptableObject
    {
        [SerializeField]
        private static LocalizationStorage localizationStorage = null;

        //идентификатор таблицы
        [SerializeField]
        private string spreadsheetId = string.Empty;
        public static string GetSpreadsheetId() { return GetSettings().spreadsheetId; }
        
        //языки
        [SerializeField]
        private List<string> languages = new List<string>();
        public static List<string> GetLanguages() { return GetSettings().languages; }
        [SerializeField]
        private int currentLanguages = 0;
        public static int GetCurrentLanguages() { return GetSettings().currentLanguages; }
        public static void SetCurrentLanguages(int value) { GetSettings().currentLanguages = value; }

        //переводы
        [SerializeField]
        private List<string> categories = new List<string>();
        public static List<string> GetCategories() { return GetSettings().categories; }

        [SerializeField]
        private List<Key> keys = new List<Key>();
        public static List<Key> GetAllKeys() { return GetSettings().keys; }
        public static List<string> GetKeys(int index) { return GetSettings().keys[index].list; }
        [Serializable]
        public struct Key
        {
            public List<string> list;
        }

        [SerializeField]
        private List<Localization> localization = new List<Localization>();
        public static List<Localization> GetLocalization() { return GetSettings().localization; }
        [Serializable]
        public struct Localization
        {
            public List<KeyItem> keys;
        }
        [Serializable]
        public struct KeyItem
        {
            public Item item;
        }
        [Serializable]
        public struct Item
        {
            public string type;
            [TextArea]
            public List<string> value;
        }

        private static LocalizationStorage GetSettings()
        {
            if (localizationStorage == null)
            {
                localizationStorage = Resources.Load("Data/Localization") as LocalizationStorage;
                Save(localizationStorage, false);
#if !UNITY_EDITOR
                Load(ref localizationStorage);
#endif
            }

            return localizationStorage;
        }

        public static void Save()
        {
            Save(GetSettings());
        }

        private static void Save(LocalizationStorage settings, bool rewrite = true)
        {
            string path = Application.streamingAssetsPath + "/Localization-" + SceneManager.GetActiveScene().name + ".json";

            if (rewrite || (!File.Exists(path) && !rewrite))
            {
                string json = JsonUtility.ToJson(settings);
                File.WriteAllText(path, json);
            }
        }

        private static void Load(ref LocalizationStorage settings)
        {
            string path = Application.persistentDataPath + "/Localization-" + SceneManager.GetActiveScene().name + ".json";
            string json = File.ReadAllText(path);
            JsonUtility.FromJsonOverwrite(json, settings);
        }

        public static bool CheckType(int category, int key, string type)
        {
            if (category < GetLocalization().Count)
            {
                if (key < GetLocalization()[category].keys.Count)
                {
                    if (GetLocalization()[category].keys[key].item.type == type)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
