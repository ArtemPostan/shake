using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
	public struct LocalizationEventContext
	{
		public SystemLanguage language;
	}

	public delegate void LocalizationEventHandler(LocalizationEventContext context);

	private const string overrideLanguagePlayerPrefsKey = "Override Language";

	public static SystemLanguage language
	{
		get
		{
			if (PlayerPrefs.HasKey("Override Language"))
			{
				return (SystemLanguage)PlayerPrefs.GetInt("Override Language");
			}
			return Application.systemLanguage;
		}
	}

	public static event LocalizationEventHandler onLanguageSettingsChange;

	public static void SetOverrideLanguage(SystemLanguage lanuage)
	{
		PlayerPrefs.SetInt("Override Language", (int)lanuage);
		LocalizationManager.onLanguageSettingsChange?.Invoke(new LocalizationEventContext
		{
			language = language
		});
	}
}
