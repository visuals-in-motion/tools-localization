using System.Collections.Generic;
using UnityEngine;
using static Visuals.LocalizationStorage;

namespace Visuals
{
	public class VisualsLocalizationComponent : MonoBehaviour, IEditor
	{
		public bool localizationEnable { get; set; } = true;
		public virtual string localizationType { get; }
		public string localizationCategory { get; set; } = "";
		protected int localizationKey { get; set; } = 0;
		public string localizationKeyName { get; set; } = "";

		[HideInInspector] public bool saveEnable = true;
		[HideInInspector] public string saveCategory = "";
		[HideInInspector] public string saveKeyName = "";

		protected int localizationCategoryIndex 
		{ 
			get 
			{ 
				return LocalizationStorage.GetCategories().IndexOf(localizationCategory); 
			}
		}
		private void OnEnable()
		{
			Enable();

			localizationEnable = saveEnable;
			localizationCategory = saveCategory;
			localizationKeyName = saveKeyName;

			LocalizationChange(LocalizationStorage.GetCurrentLanguages(), false);
		}

		protected virtual void Enable()
		{

		}
		private void Update()
		{
			if (!localizationEnable) return;

			if (LocalizationStorage.CheckType(localizationCategory, localizationKeyName, localizationType))
			{
				LocalizationChange(LocalizationStorage.GetCurrentLanguages(), false);
			}
		}

		protected virtual void SetValues(string value)
		{
			
		}
		#region Public methods
		public virtual Localization GetCurrentCategoryInfo()
		{
			if(localizationCategoryIndex >= 0)
			{
				return LocalizationStorage.GetLocalization()[localizationCategoryIndex];
			}
			return new Localization();
		}
		public virtual List<KeyItem> GetCurrentKeysInfo()
		{
			return GetCurrentCategoryInfo().keys;
		}
		public virtual KeyItem GetCurrentKeyInfo()
		{
			List<KeyItem> keysItem = GetCurrentCategoryInfo().keys;
			if(localizationKey < keysItem.Count)
				return GetCurrentCategoryInfo().keys[localizationKey];
			return new KeyItem();
		}
		public virtual List<string> GetCurrentValuesInfo()
		{
			return GetCurrentKeysInfo()[localizationKey].value;
		}
		public virtual void LocalizationLoad()
		{
			if (!localizationEnable) return;
		}
		public virtual void LocalizationChange(int languageIndex, bool changeValues)
		{
			if (!localizationEnable) return;

			localizationKey = GetKeyIndexByName(localizationKeyName);

			List<string> values = GetCurrentValuesInfo();
			if(values != null)
			{
				SetValues(values[languageIndex]);
				if (changeValues)
				{
					LocalizationLoad();
				}
			}
		}
		public List<string> GetValuesByKeyIndex(int index)
		{
			if (localizationCategoryIndex >= 0)
			{
				return GetCurrentKeysInfo()[index].value; 
			}
			else
				return null;
		}
		public bool SetKeyByIndex(int index)
		{
			if(localizationCategoryIndex >= 0)
			{
				if (index >= 0 && index < GetCurrentKeysInfo().Count)
				{
					localizationKey = index;
					localizationKeyName = saveKeyName = GetCurrentKeyName();

					LocalizationChange(LocalizationStorage.GetCurrentLanguages(), true);
					return true;
				}
			}
			
			return false;
		}

		public bool SetKeyByName(string keyName)
		{
			int index = GetKeyIndexByName(keyName);

			return SetKeyByIndex(index);
		}
		public int GetCurrentKeyIndex()
		{
			if(localizationCategoryIndex >= 0)
			{
				return GetCurrentKeysInfo().FindIndex(k => k.name == localizationKeyName);
			}
			return -1;
		}
		public string GetCurrentKeyName()
		{
			return GetCurrentCategoryAllKeys()[localizationKey];
		}
		public List<string> GetCurrentCategoryAllKeys()
		{
			return LocalizationStorage.GetKeys(localizationCategoryIndex);
		}
		public List<string> GetCurrentCategoryKeysByType(string type)
		{
			List<string> list = new List<string>();
			if(localizationCategoryIndex >= 0)
			{
				List<KeyItem> keys = GetCurrentKeysInfo();
				for (int i = 0; i < keys.Count; i++)
				{
					if (keys[i].type == type)
					{
						list.Add(keys[i].name);
					}
				}
			}
			
			return list;
		}
		public int GetKeyIndexByName(string keyName)
		{
			if (localizationCategoryIndex >= 0)
			{
				int keyIndex = LocalizationStorage.GetAllKeys()[localizationCategoryIndex].list.IndexOf(keyName);
				return keyIndex;
			}
			else
				return -1;
		}
		public string GetKeyNameByIndex(int keyIndex)
		{
			if (localizationCategoryIndex >= 0)
			{
				List<KeyItem> keys = GetCurrentKeysInfo();
				if (keyIndex >= 0 && keyIndex < keys.Count)
				{
					return keys[keyIndex].name;
				}
			}
			
			return null;
		}
		
		public string GetCurrentCategoryName()
		{
			return GetCurrentCategoryInfo().categoryName;
		}
		public int GetCurrentCategoryIndex()
		{
			return localizationCategoryIndex;
		}
		public bool SetCategoryByName(string categoryName)
		{
			int categoryIndex = LocalizationStorage.GetCategories().IndexOf(categoryName);
			return SetCategoryByIndex(categoryIndex);
		}
		public bool SetCategoryByIndex(int index)
		{
			if (index >= 0 && index < LocalizationStorage.GetCategories().Count)
			{
				localizationCategory = saveCategory = LocalizationStorage.GetCategories()[index];
				SetKeyByIndex(0);
				return true;
			}
			return false;
		}
		#endregion
	}
}