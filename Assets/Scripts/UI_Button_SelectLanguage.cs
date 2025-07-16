using UnityEngine;
using UnityEngine.UI;

public class UI_Button_SelectLanguage : MonoBehaviour
{
	private Button button;

	public SystemLanguage language;

	public Image checkBox;

	private void Awake()
	{
		button = GetComponent<Button>();
		LocalizationManager.onLanguageSettingsChange += OnLanguageSettingsChange;
		button.onClick.AddListener(OnButtonClick);
	}

	private void OnDestroy()
	{
		LocalizationManager.onLanguageSettingsChange -= OnLanguageSettingsChange;
	}

	private void Start()
	{
		Refresh();
	}

	private void OnLanguageSettingsChange(LocalizationManager.LocalizationEventContext context)
	{
		Refresh();
	}

	private void Refresh()
	{
		Color32 c = checkBox.color;
		if (LocalizationManager.language == language)
		{
			c.a = byte.MaxValue;
		}
		else
		{
			c.a = 0;
		}
		checkBox.color = c;
	}

	private void OnButtonClick()
	{
		LocalizationManager.SetOverrideLanguage(language);
	}
}
