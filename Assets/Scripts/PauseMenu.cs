using UnityEngine;

public class PauseMenu : MonoBehaviour
{
	private CanvasGroup canvasGroup;

	private bool show;

	[SerializeField]
	private float showHideSpeed = 1f;

	private void Awake()
	{
		canvasGroup = GetComponent<CanvasGroup>();
	}

	private void Update()
	{
		UpdateStatus();
		UpdateCanvasGroupProperties();
	}

	private void UpdateStatus()
	{
		if ((bool)LevelManager.instance && LevelManager.instance.GameState == LevelManager.gameStates.playing)
		{
			if (LevelManager.Paused)
			{
				show = true;
			}
			else
			{
				show = false;
			}
		}
		else
		{
			show = false;
		}
	}

	private void UpdateCanvasGroupProperties()
	{
		if (show)
		{
			canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 1f, showHideSpeed * Time.unscaledDeltaTime);
			canvasGroup.blocksRaycasts = true;
		}
		else
		{
			canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 0f, showHideSpeed * Time.unscaledDeltaTime);
			canvasGroup.blocksRaycasts = false;
		}
	}
}
