using UnityEngine;
using UnityEngine.UI;

public class LevelTimerHUD : MonoBehaviour
{
	public CanvasGroup canvasGroup;

	public Text text;

	private void Update()
	{
		if ((bool)LevelTimer.activeLevelTimer)
		{
			canvasGroup.alpha = 1f;
			text.text = LevelTimer.activeLevelTimer.parsedRemainingTime;
		}
		else
		{
			canvasGroup.alpha = 0f;
		}
	}
}
