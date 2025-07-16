using System;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{
	public delegate void LevelTimerEventHandler();

	public static LevelTimer activeLevelTimer;

	public Timer timer;

	public float levelTime = 300f;

	public bool pauseOnAwake;

	public float remainingTime
	{
		get
		{
			float num = levelTime - timer.time;
			if (num < 0f)
			{
				num = 0f;
			}
			return num;
		}
	}

	public string parsedRemainingTime => TimeSpan.FromSeconds(remainingTime).ToString("mm\\:ss\\.fff");

	public bool overTime => remainingTime <= 0f;

	public static event LevelTimerEventHandler onLevelTimeUp;

	private void Awake()
	{
		timer = new Timer();
		activeLevelTimer = this;
		if (pauseOnAwake)
		{
			timer.Pause();
		}
	}

	private void Update()
	{
		if (!(activeLevelTimer != this) && !timer.paused && timer.time > levelTime)
		{
			timer.Pause();
			TimesUp();
		}
	}

	public void Pause()
	{
		timer.Pause();
	}

	public void Resume()
	{
		if (!overTime)
		{
			timer.Resume();
		}
	}

	public void Reset()
	{
		timer.ResetTime();
	}

	private void TimesUp()
	{
		LevelTimer.onLevelTimeUp?.Invoke();
		GameManager.Instance.LevelManager.Defeat(DefeatType.timeUp);
	}

	public static void StartLevelTimer()
	{
		if ((bool)activeLevelTimer)
		{
			activeLevelTimer.Resume();
		}
	}
}
