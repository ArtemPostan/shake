using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizationSync : MonoBehaviour
{
	public enum SyncTypes
	{
		SetActive,
		text,
		textMesh
	}

	public enum SetTypes
	{
		auto,
		chinese,
		english
	}

	public SyncTypes syncType = SyncTypes.textMesh;

	public List<GameObject> ChineseObjects;

	public List<GameObject> EnglishObjects;

	public Text text;

	public TextMesh textMesh;

	[Multiline(5)]
	public string ChineseText;

	[Multiline(5)]
	public string EnglishText;

	private bool syncType_SetActive => syncType == SyncTypes.SetActive;

	private bool syncType_text => syncType == SyncTypes.text;

	private bool syncType_textmesh => syncType == SyncTypes.textMesh;

	private bool textOrTextMesh
	{
		get
		{
			if (!syncType_text)
			{
				return syncType_textmesh;
			}
			return true;
		}
	}

	private void Awake()
	{
		LocalizationManager.onLanguageSettingsChange += OnLanguageSettingsChange;
	}

	private void OnDestroy()
	{
		LocalizationManager.onLanguageSettingsChange -= OnLanguageSettingsChange;
	}

	private void OnLanguageSettingsChange(LocalizationManager.LocalizationEventContext context)
	{
		Set();
	}

	private void Start()
	{
		Set();
	}

	public static bool IsChinese()
	{
		if (LocalizationManager.language != SystemLanguage.Chinese && LocalizationManager.language != SystemLanguage.ChineseSimplified)
		{
			return LocalizationManager.language == SystemLanguage.ChineseTraditional;
		}
		return true;
	}

	public void Set(SetTypes _setType = SetTypes.auto)
	{
		bool flag = false;
		switch (_setType)
		{
		case SetTypes.auto:
			flag = IsChinese();
			break;
		case SetTypes.chinese:
			flag = true;
			break;
		case SetTypes.english:
			flag = false;
			break;
		}
		switch (syncType)
		{
		case SyncTypes.SetActive:
			for (int i = 0; i < EnglishObjects.Count; i++)
			{
				EnglishObjects[i]?.SetActive(!flag);
			}
			for (int j = 0; j < ChineseObjects.Count; j++)
			{
				ChineseObjects[j]?.SetActive(flag);
			}
			break;
		case SyncTypes.text:
			if (text == null)
			{
				text = GetComponent<Text>();
				if (text == null)
				{
					break;
				}
			}
			if (flag)
			{
				text.text = ChineseText;
			}
			else
			{
				text.text = EnglishText;
			}
			break;
		case SyncTypes.textMesh:
			if (textMesh == null)
			{
				textMesh = GetComponent<TextMesh>();
				if (textMesh == null)
				{
					break;
				}
			}
			if (flag)
			{
				textMesh.text = ChineseText;
			}
			else
			{
				textMesh.text = EnglishText;
			}
			break;
		}
	}

	public void ForceSetToChinese()
	{
		Set(SetTypes.chinese);
	}

	public void ForceSetToEnglish()
	{
		Set(SetTypes.english);
	}
}
