using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Visuals
{
	[ExecuteAlways]
	public class VisualsLocalizationText : VisualsLocalizationComponent
	{
		private Text textComponent = null;
		private TMP_Text tmpComponent = null;
		public override string localizationType { get; } = "text";
		
		private string defaultSeparatorCharacter = "$";
		protected override void Enable()
		{
			textComponent = GetComponent<Text>();
			tmpComponent = GetComponent<TMP_Text>();
		}
		protected override void SetValues(string value)
		{
			if (textComponent != null)
			{
				textComponent.text = value;
			}

			if (tmpComponent != null)
			{
				tmpComponent.text = value;
			}
		}
		#region Public methods

		public void ReplaceSubstringInText(string newString, string separatorCharacter)
		{
			var values = GetValuesByKeyIndex(localizationKey);
			for (int i = 0; i < values.Count; i++)
			{
				string character = "";
				if (values[i].IndexOf(defaultSeparatorCharacter) != -1)
				{
					character = defaultSeparatorCharacter;
				}
				else
				{
					character = separatorCharacter;
				}
				values[i] = values[i].Replace(character, newString);
			}
			SetValues(values[LocalizationStorage.GetCurrentLanguages()]);
		}
		#endregion
	}
}