using UnityEngine;

public class UI_AdditionalInfo : MonoBehaviour
{
	public GameObject achievementDisabledHUD;

	private void Awake()
	{
		FriendlyDamageSwitch.onFriendlyDamageStatusChanged += OnFriendlyDamageStatusChanged;
		CheatModeSwitch.onCheatModeChanged += OnCheatStatusChanged;
	}

	private void OnDestroy()
	{
		FriendlyDamageSwitch.onFriendlyDamageStatusChanged -= OnFriendlyDamageStatusChanged;
		CheatModeSwitch.onCheatModeChanged -= OnCheatStatusChanged;
	}

	private void OnFriendlyDamageStatusChanged(FriendlyDamageSwitch.FriendlyDamageSwitchEventContext context)
	{
		Refresh();
	}

	private void OnCheatStatusChanged(CheatModeSwitch.CheatModeSwitchEventContext context)
	{
		Refresh();
	}

	private void Refresh()
	{
		achievementDisabledHUD.SetActive(!DataManager.AchievementAndStatisticsEnabled);
	}

	private void Start()
	{
		Refresh();
	}
}
