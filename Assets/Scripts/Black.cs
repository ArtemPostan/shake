using UnityEngine;

public class Black : MonoBehaviour
{
	public Combat combat;

	private bool dead;

	private void Start()
	{
	}

	private void OnEnable()
	{
		combat.SetAsBlack();
		Combat.KillAllEnemies();
	}

	private void Update()
	{
		if (!dead && combat.IsDead)
		{
			dead = true;
			DataManager.data.theMythDefeated = true;
			DataManager.Save();
			DataManager.SetAchievement("defeatTheMyth");
		}
	}
}
