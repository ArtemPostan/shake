using UnityEngine;

public class MeleeWeaponDamage : MonoBehaviour
{
	private int attackID;

	public int attackDamage;

	public int attackRight;

	public int team;

	public int AttackID => attackID;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnEnable()
	{
		attackID++;
		UnityEngine.Debug.Log("attack:" + AttackID.ToString());
	}
}
