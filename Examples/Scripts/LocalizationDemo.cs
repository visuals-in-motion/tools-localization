using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Visuals;
public class LocalizationDemo : MonoBehaviour
{
	[SerializeField] private VisualsLocalizationText[] visualsLocalizationTexts;
	[SerializeField] private InputField categoryName;
	[SerializeField] private InputField keyName;
	[SerializeField] private InputField keyType;
	[SerializeField] private InputField rus;
	[SerializeField] private InputField eng;

	[SerializeField] private InputField keyNameOrIndex;
	[SerializeField] private VisualsLocalizationText dunamicText;
	private void OnDestroy()
	{
		VisualsLocalization.loadLocalization -= OnLoadLocalization;
	}
	private void Start()
	{
		LocalizationStorage.SetSpreadsheetURL("https://docs.google.com/spreadsheets/d/1330L_ukIK5H8jzJHr2IsRSIuFCQP_THqTV782TmvC3U/edit#gid=883927456");
		VisualsLocalization.Import();
		VisualsLocalization.loadLocalization += OnLoadLocalization;
	}
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.K))
		{
			VisualsLocalization.UpdateExistingSheets();
		}
	}
	private void OnLoadLocalization()
	{
		for (int i = 0; i < visualsLocalizationTexts.Length; i++)
		{
			visualsLocalizationTexts[i].SetKeyByIndex(i);
		}
	}

	public void OnChangeLanguage()
	{
		if(LocalizationStorage.GetCurrentLanguages() == 0)
		{
			LocalizationStorage.SetCurrentLanguages(1);
		}
		else
		{
			LocalizationStorage.SetCurrentLanguages(0);
		}
	}
	public void OnAddNewCategory()
	{
		if(categoryName.text != string.Empty)
		{
			LocalizationStorage.AddCategory(categoryName.text);
		}
	}
	public void OnAddNewKey()
	{
		if(keyName.text != string.Empty && keyType.text != string.Empty)
		{
			
			LocalizationStorage.AddKeyValues(categoryName.text, keyName.text, keyType.text, new List<string>() {eng.text, rus.text });
		}
	}

	public void SetKey()
	{
		if(categoryName.text != string.Empty)
		{
			dunamicText.SetCategoryByName(categoryName.text);
		}
		
		int index;
		bool success = int.TryParse(keyNameOrIndex.text, out index);
		if (success)
		{
			dunamicText.SetKeyByIndex(index);
		}
		else
		{
			dunamicText.SetKeyByName(keyNameOrIndex.text);
		}
	}
}
