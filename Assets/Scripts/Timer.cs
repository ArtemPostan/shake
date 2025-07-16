using UnityEngine;

public class Timer
{
	private float startTime;

	private float startFixedTime;

	private float startUnscaledTime;

	private float startFixedUnscaledTime;

	private float pauseBuffer_time;

	private float pauseBuffer_fixedTime;

	private float pauseBuffer_unscaledTime;

	private float pauseBuffer_fixedUnscaledTime;

	public bool paused
	{
		get;
		private set;
	}

	public float time
	{
		get
		{
			if (paused)
			{
				return pauseBuffer_time;
			}
			return Time.time - startTime;
		}
	}

	public float fixedTime
	{
		get
		{
			if (paused)
			{
				return pauseBuffer_fixedTime;
			}
			return Time.fixedTime - startFixedTime;
		}
	}

	public float unscaledTime
	{
		get
		{
			if (paused)
			{
				return pauseBuffer_unscaledTime;
			}
			return Time.unscaledTime - startUnscaledTime;
		}
	}

	public float fixedUnscaledTime
	{
		get
		{
			if (paused)
			{
				return pauseBuffer_fixedUnscaledTime;
			}
			return Time.fixedUnscaledTime - startFixedUnscaledTime;
		}
	}

	public void ResetTime()
	{
		startTime = Time.time;
		startFixedTime = Time.fixedTime;
		startUnscaledTime = Time.unscaledTime;
		startFixedUnscaledTime = Time.fixedUnscaledTime;
		SetPauseBufferAsCurrentTime();
	}

	public Timer()
	{
		ResetTime();
	}

	public void SetTime(float time)
	{
		startTime = Time.time - time;
		startFixedTime = Time.fixedTime - time;
		startUnscaledTime = Time.unscaledTime - time;
		startFixedUnscaledTime = Time.fixedUnscaledTime - time;
		SetPauseBufferAsCurrentTime();
	}

	public void SetPauseBufferAsCurrentTime()
	{
		pauseBuffer_time = time;
		pauseBuffer_fixedTime = fixedTime;
		pauseBuffer_unscaledTime = unscaledTime;
		pauseBuffer_fixedUnscaledTime = fixedUnscaledTime;
	}

	public void Pause()
	{
		paused = true;
		SetPauseBufferAsCurrentTime();
	}

	public void Resume()
	{
		if (paused)
		{
			paused = false;
			startTime = Time.time - pauseBuffer_time;
			startFixedTime = Time.fixedTime - pauseBuffer_fixedTime;
			startUnscaledTime = Time.unscaledTime - pauseBuffer_unscaledTime;
			startFixedUnscaledTime = Time.fixedUnscaledTime - pauseBuffer_fixedUnscaledTime;
		}
	}
}
