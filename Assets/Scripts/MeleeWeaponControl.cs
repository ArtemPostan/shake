using UnityEngine;

public class MeleeWeaponControl : MonoBehaviour
{
	public Animator animator;

	private int attackState = 1;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void Attack()
	{
		if ((bool)animator)
		{
			if (attackState == 1)
			{
				animator.SetTrigger("attack1");
				attackState = 2;
			}
			else
			{
				animator.SetTrigger("attack2");
				attackState = 1;
			}
		}
	}
}
