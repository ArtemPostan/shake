using System;
using System.Collections.Generic;
using UnityEngine;

public class PeriodicalBehaviour : MonoBehaviour
{
	[Serializable]
	public struct Float01
	{
		[Range(0f, 1f)]
		public float value;

		public static implicit operator float(Float01 value)
		{
			return value.value;
		}

		public static implicit operator Float01(float value)
		{
			return new Float01(value);
		}

		private Float01(float value)
		{
			this.value = value;
		}
	}

	public float period = 1f;

	public List<Float01> ticks;

	public bool useFixedTime;

	public bool overrideTimer;

	public TimerContainer _timerContainer;

	public float offset;

	public bool waitForFirstTick;

	private bool firstTickTicked;

	public bool playing = true;

	public bool stopAfterCurrentPeriod;

	protected float lastFrameTime;

	public float currentTime
	{
		get
		{
			if (timer == null)
			{
				return offset;
			}
			return (useFixedTime ? timer.fixedTime : timer.time) + offset;
		}
	}

	public Timer timer
	{
		get
		{
			if (overrideTimer)
			{
				if (_timerContainer == null)
				{
					UnityEngine.Debug.LogError("No Timer reference assigned.");
				}
				return _timerContainer.timer;
			}
			return null;
		}
	}

	public float t
	{
		get
		{
			float currentPeriodStartPosition = GetCurrentPeriodStartPosition(currentTime, period);
			return currentTime - currentPeriodStartPosition;
		}
	}

	public float t01 => t / period;

	public virtual void Tick(int i)
	{
	}

	private void Awake()
	{
		lastFrameTime = offset;
		firstTickTicked = false;
	}

	public void ResetStats()
	{
		firstTickTicked = false;
	}

	private void OnEnable()
	{
		lastFrameTime = currentTime;
	}

	private void Start()
	{
		lastFrameTime = currentTime;
	}

	private void TicksUpdate()
	{
		if (period == 0f)
		{
			return;
		}
		if (period < 0f)
		{
			period = 0f - period;
		}
		float currentPeriodStartPosition = GetCurrentPeriodStartPosition(currentTime, period);
		float num = lastFrameTime - currentPeriodStartPosition;
		float num3 = num / period;
		bool flag = num < 0f;
		if (stopAfterCurrentPeriod && flag)
		{
			playing = false;
		}
		for (int i = 0; i < ticks.Count; i++)
		{
			Float01 value = ticks[i];
			float num2 = currentPeriodStartPosition + (float)value * period;
			bool flag2 = num2 >= lastFrameTime && num2 < currentTime;
			bool flag3 = flag && num2 - period >= lastFrameTime && num2 - period < currentTime;
			if ((stopAfterCurrentPeriod && flag) & flag2)
			{
				flag2 = false;
			}
			if (flag2 | flag3)
			{
				if (waitForFirstTick && !firstTickTicked && i == 0)
				{
					firstTickTicked = true;
				}
				if (!waitForFirstTick || firstTickTicked)
				{
					Tick(i);
				}
			}
		}
		lastFrameTime = currentTime;
	}

	protected virtual void Update()
	{
		if (playing && !useFixedTime)
		{
			TicksUpdate();
		}
	}

	protected virtual void FixedUpdate()
	{
		if (playing && useFixedTime)
		{
			TicksUpdate();
		}
	}

	private static float GetCurrentPeriodStartPosition(float currentTime, float period)
	{
		if (period == 0f)
		{
			return 0f;
		}
		float num = currentTime;
		if (num < 0f)
		{
			num = 0f - num;
		}
		if (period < 0f)
		{
			period = 0f - period;
		}
		int num2 = Mathf.FloorToInt(num / period);
		if (currentTime < 0f)
		{
			num2++;
			num2 = -num2;
		}
		return (float)num2 * period;
	}

	public void StopPlayingAfterCurrentPeriod()
	{
		stopAfterCurrentPeriod = true;
	}

	public void Play()
	{
		stopAfterCurrentPeriod = false;
		playing = true;
		lastFrameTime = currentTime;
	}
}
