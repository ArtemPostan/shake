using UnityEngine;

public class ActiveByConditions : MonoBehaviour
{
	public GameObject target;

	public bool inverse;

	[Header("关卡")]
	public bool byLevel = true;

	public bool byEndLevel;

	public int index;

	[Header("大坏蛋")]
	public bool byBadAssFinished;

	[Header("杀够100个")]
	public bool byKill100;

	[Header("小黑")]
	public bool byMythDefeated;

	public bool byMythFinished;

	private bool needLevelIndex
	{
		get
		{
			if (byLevel)
			{
				return !byEndLevel;
			}
			return false;
		}
	}

	private void Start()
	{
		bool flag = true;
		if (byLevel)
		{
			if (byEndLevel)
			{
				index = 7;
			}
			if (!DataManager.EverEnteredLevel("Level" + index.ToString()))
			{
				flag = false;
			}
		}
		if (byBadAssFinished && !DataManager.data.badAssFinished)
		{
			flag = false;
		}
		if (byKill100 && !DataManager.data.suitManUnlocked)
		{
			flag = false;
		}
		if (byMythDefeated && !DataManager.data.theMythDefeated)
		{
			flag = false;
		}
		if (byMythFinished && !DataManager.data.theMythFinished)
		{
			flag = false;
		}
		if (flag)
		{
			target.SetActive(!inverse);
		}
		else
		{
			target.SetActive(inverse);
		}
	}

	private void Update()
	{
	}
}
