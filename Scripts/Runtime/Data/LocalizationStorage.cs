using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Visuals
{
    [Serializable]
    public class LocalizationStorage : ScriptableObject
    {
        [SerializeField]
        private static LocalizationStorage localizationStorage = null;

        //информация о таблице
        [SerializeField]
        private string spreadsheetURL = string.Empty;
        public static string GetSpreadsheetURL() { return GetSettings().spreadsheetURL; }
        public static void SetSpreadsheetURL(string value) { GetSettings().spreadsheetURL = value; }
        private string spreadsheetId = string.Empty;
        public static string GetSpreadsheetId() { return GetSettings().spreadsheetId; }
        public static void SetSpreadsheetId(string id) { GetSettings().spreadsheetId = id; }
        //языки
        [SerializeField]
        private List<string> languages = new List<string>();
        public static List<string> GetLanguages() { return GetSettings().languages; }
        [SerializeField]
        private int currentLanguages = 0;
        public static int GetCurrentLanguages() { return GetSettings().currentLanguages; }
        public static void SetCurrentLanguages(int value) 
        {
            GetSettings().currentLanguages = value;
            changedLanguage?.Invoke(GetSettings().currentLanguages);
        }
        public static event Action<int> changedLanguage;
        //переводы
        
        private List<SheetsInfo> categoriesInfo = new List<SheetsInfo>();
        
        public static List<SheetsInfo> GetCategoriesInfo() { return (GetSettings() != null) ? GetSettings().categoriesInfo : null; }
        public static void SetCategoriesInfo(List<SheetsInfo> value) { GetSettings().categoriesInfo = value; }
        
        private List<string> categories = new List<string>();
        public static List<string> GetCategories() { return (GetSettings() != null) ? GetSettings().categories : null; }

        
        private List<Key> keys = new List<Key>();
        public static List<Key> GetAllKeys() { return GetSettings().keys; }
        public static List<string> GetKeys(int index) { return GetSettings().keys[index].list; }
        [SerializeField]  
        private List<string> allTypes = new List<string>();
        public static List<string> GetAllTypes() { return GetSettings().allTypes;}
        public static void AddTypes(string type) 
        { 
            if(!GetAllTypes().Contains(type))
			{
                GetAllTypes().Add(type);
			}
        }
        public static void ClearAllTypes()
		{
            GetAllTypes().Clear();
            GetCategoriesByType().Clear();
        }
        [SerializeField]
        private List<CategoriesType> categoriesByType = new List<CategoriesType>();
        
        public static List<CategoriesType> GetCategoriesByType() { return GetSettings().categoriesByType; }
        [Serializable]
        public struct Key
        {
            public List<string> list;
        }

        [SerializeField]
        private List<Localization> localization = new List<Localization>();
        public static List<Localization> GetLocalization() { return (GetSettings() != null) ? GetSettings().localization : null; }
        [Serializable]
        public struct Localization
        {
            public string categoryName;
            public List<KeyItem> keys;
        }
        [Serializable]
        public struct KeyItem
        {
            public string name;
            public string type;
            [TextArea]
            public List<string> value;

            public KeyItem(string name, string type, List<string> value)
			{
                this.name = name;
                this.type = type;
                this.value = value;
			}
        }
        [Serializable] public struct CategoriesType
		{
            public string type;
            public List<string> categories;

            public CategoriesType(string type, List<string> categories)
			{
                this.type=type;
                this.categories = categories;
			}
        }
        private static LocalizationStorage GetSettings()
        {
            if (localizationStorage == null)
            {
                localizationStorage = Resources.Load("Localization") as LocalizationStorage;
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

        private static void Save(LocalizationStorage settings, bool rewrite = true, bool multiScene = false)
        {
            string scene = (multiScene) ? SceneManager.GetActiveScene().name : "Main";
            string path = Application.streamingAssetsPath + "/Localization-" + scene + ".json";
            
            if (rewrite || (!File.Exists(path) && !rewrite))
            {
                if (!File.Exists(Application.streamingAssetsPath)) Directory.CreateDirectory(Application.streamingAssetsPath);

                string json = JsonUtility.ToJson(settings);
                File.WriteAllText(path, json);
#if UNITY_EDITOR
                EditorUtility.SetDirty(settings);
#endif
            }
        }

        private static void Load(ref LocalizationStorage settings)
        {
            string path = Application.streamingAssetsPath + "/Localization-" + SceneManager.GetActiveScene().name + ".json";
            if (!File.Exists(path)) path = Application.streamingAssetsPath + "/Localization-Main.json";
            string json = File.ReadAllText(path);
            JsonUtility.FromJsonOverwrite(json, settings);
        }

        public static bool CheckType(string categoryName, string key, string type)
        {
            int category = GetCategories().IndexOf(categoryName);
            if (GetLocalization() != null && category < GetLocalization().Count && category >= 0)
            {
                int keyIndex = GetAllKeys()[category].list.IndexOf(key);
                if (keyIndex < GetLocalization()[category].keys.Count)
                {
                    if (GetLocalization()[category].keys[keyIndex].type == type)
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

        public static void AddKeyValues(string categoryName, string keyName, string type, List<string> values)
		{
            int categoryIndex = GetCategories().IndexOf(categoryName);
            
            GetAllKeys()[categoryIndex].list.Add(keyName);
            GetLocalization()[categoryIndex].keys.Add(new KeyItem(keyName, type, values)) ;
		}
        public static void AddCategory(string categoryName)
		{
            Localization localization = new Localization();
            localization.categoryName = categoryName;
            localization.keys = new List<KeyItem>();

            GetLocalization().Add(localization);
            GetCategories().Add(categoryName);

            Key newKey = new Key();
            newKey.list = new List<string>();
            GetAllKeys().Add(newKey);
        }
        public static void SetCategoriesByType()
		{
            List<string> allTypes = GetAllTypes();
            for (int i = 0; i < allTypes.Count; i++)
			{
                CategoriesType categoriesType = new CategoriesType(allTypes[i], CategoriesWithKeysType(allTypes[i]));
                if (!GetCategoriesByType().Contains(categoriesType))
                {
                    GetCategoriesByType().Add(categoriesType); 
                }
            }
		}
        private static List<string> CategoriesWithKeysType(string type)
		{
            List<string> result = new List<string>();
            
            List<Localization> allCategories = GetLocalization();

            for(int i = 0; i < allCategories.Count; i ++)
			{
                string currentCategoryName = allCategories[i].categoryName;
                List<KeyItem> keys = allCategories[i].keys;
                for(int j = 0; j < keys.Count; j ++)
				{
                    if(keys[j].type == type && !result.Contains(currentCategoryName))
					{
                        result.Add(currentCategoryName);
					}
				}
			}

            return result;
		}
        public static List<string> GetCategoriesWithKeysType(string type)
		{
            List<CategoriesType> categoriesType = GetCategoriesByType();
            List<string> result = new List<string>();
            bool isContains = false;
            for (int i = 0; i < categoriesType.Count; i ++)
			{
                if(categoriesType[i].type == type)
				{
                    isContains = true;
                    result = categoriesType[i].categories;
                }
			}
            if(isContains)
			{
                return result;  
			}
            return new List<string>();
        }
    }
}
