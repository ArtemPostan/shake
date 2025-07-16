using UnityEngine;

public class TimerContainer : MonoBehaviour
{
	private Timer _timer;

	public bool pauseOnAwake;

	public Timer timer
	{
		get
		{
			if (_timer == null)
			{
				_timer = new Timer();
			}
			return _timer;
		}
	}

	private void Awake()
	{
		if (pauseOnAwake)
		{
			Pause();
		}
	}

	public void ResetTimer()
	{
		timer.ResetTime();
	}

	public void Pause()
	{
		timer.Pause();
	}

	public void Resume()
	{
		timer.Resume();
	}
}
